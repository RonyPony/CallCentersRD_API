using CallCentersRD_API.Database.Entities.Base;
using Newtonsoft.Json;

namespace CallCentersRD_API.Database.DTOs.Base;

public abstract class BaseDto<TEntity, TKey> where TEntity : class, IBaseEntity<TKey> where TKey : IEquatable<TKey>
{
    public BaseDto()
    {

    }

    public BaseDto(TEntity entity)
    {
        Id = entity.Id;
        CreatedAt = entity.CreatedAt;
        UpdatedAt = entity.UpdatedAt;
    }

    public TKey Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    [JsonIgnore]
    public TEntity Entity
    {
        get
        {
            var ent = MakeEntity();

            ent.Id = Id;

            return ent;
        }
    }

    protected abstract TEntity MakeEntity();
}
