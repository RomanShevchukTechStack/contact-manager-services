using Ardalis.Result;
using ContactManager.Data.Entities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Repository<T>  where T : BaseEntity
{
  private readonly string filePath;
  private List<T> items;

  public Repository(string filePath = "repository.json")
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

  public Result<List<T>> GetAll()
  {
    return Result<List<T>>.Success(items);
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
}
