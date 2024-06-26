using Ardalis.Result;
using ContactManager.Data.Entities;
using ContactManager.Data.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

public class Repository<T> where T : BaseEntity
{
  private readonly string filePath;
  private List<T> items;

  public Repository(string filePath = "contacts.json")
  {
    this.filePath = filePath;

    if (File.Exists(filePath))
    {
      var json = File.ReadAllText(filePath);
      items = JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
    }
    else
    {
      items = new List<T>();
    }
  }

  public Result<List<T>> GetAll(FilterOptions? filterOptions = null, PaginationOptions? paginationOptions = null, OrderOptions? orderOptions = null)
  {
    List<T> items = this.items;

    if (filterOptions != null)
    {
      items = ApplyFiltering(items, filterOptions.SearchValue);
    }

    if (paginationOptions != null)
    {
      items = ApplyPagination(items, paginationOptions.Page, paginationOptions.Count);

    }

    if (orderOptions != null)
    {
      items = ApplyOrdering(items, orderOptions);
    }

    return Result<List<T>>.Success(items);
  }

  private List<T> ApplyFiltering(List<T> items, string? searchValue)
  {
    if (string.IsNullOrEmpty(searchValue))
    {
      return items;
    }

    string lowercaseSearchValue = searchValue.ToLower();
    return items.Where(item => ContainsSearchValue(item, lowercaseSearchValue)).ToList();
  }

  private bool ContainsSearchValue(T item, string lowercaseSearchValue)
  {
    var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
    foreach (var property in properties)
    {
      if (property.PropertyType == typeof(string))
      {
        string value = (string)property.GetValue(item);
        if (value?.ToLower().Contains(lowercaseSearchValue) == true)
        {
          return true;
        }
      }
    }
    return false;
  }

  private List<T> ApplyPagination(List<T> items, int page, int count)
  {
    int skip = (page - 1) * count;
    int take = count;
    return items.Skip(skip).Take(take).ToList();
  }

  private List<T> ApplyOrdering(List<T> items, OrderOptions orderOptions)
  {

    if (HasProperty<T>(orderOptions.OrderBy))
    {
      var propertyInfo = typeof(T).GetProperty(orderOptions.OrderBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
      if (orderOptions.OrderDirection.Equals("desc", StringComparison.InvariantCultureIgnoreCase))
        return propertyInfo != null ? items.OrderByDescending(item => propertyInfo.GetValue(item, null)).ToList() : items;
      else
        return propertyInfo != null ? items.OrderBy(item => propertyInfo.GetValue(item, null)).ToList() : items;

    }

    return items;
  }

  public Result<T> GetById(int id)
  {
    var item = items.FirstOrDefault(i => i.Id == id);
    return item != null ? Result<T>.Success(item) : Result<T>.NotFound();
  }

  public Result<T> Add(T entity)
  {
    entity.Id = items.Any() ? items.Max(i => i.Id) + 1 : 1;
    items.Add(entity);
    SaveChanges();
    return Result<T>.Success(entity);
  }

  public Result<T> Update(T entity)
  {
    var existing = GetById(entity.Id).Value;
    if (existing != null)
    {
      var index = items.IndexOf(existing);
      items[index] = entity;
      SaveChanges();
      return Result<T>.Success(entity);
    }
    return Result<T>.NotFound();
  }

  public Result Delete(int id)
  {
    var entity = GetById(id).Value;
    if (entity != null)
    {
      items.Remove(entity);
      SaveChanges();
      return Result.Success();
    }
    return Result.NotFound();
  }

  private void SaveChanges()
  {
    var json = JsonConvert.SerializeObject(items, Formatting.Indented);
    File.WriteAllText(filePath, json);
  }

  public bool HasProperty<T>(string propertyName)
  {
    Type type = typeof(T);
    PropertyInfo propertyInfo = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
    return propertyInfo != null;
  }
}
