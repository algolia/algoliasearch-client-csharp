using System.Collections.Generic;
using System.Linq;

namespace Algolia.Search.Models.Search;

public partial class ApiKey
{
  /// <summary>
  /// Compare two ApiKey objects
  /// </summary>
  /// <param name="obj"></param>
  /// <returns></returns>
  public override bool Equals(object obj)
  {
    // We compare the properties of the object
    // We DO NOT compare the null props of the obj.
    if (obj is ApiKey other)
    {
      return
        Description == other.Description &&
        QueryParameters == other.QueryParameters &&
        CheckSequence(Acl, other.Acl) &&
        CheckSequence(Indexes, other.Indexes) &&
        CheckSequence(Referers, other.Referers) &&
        CheckNullable(MaxHitsPerQuery, other.MaxHitsPerQuery) &&
        CheckNullable(MaxQueriesPerIPPerHour, other.MaxQueriesPerIPPerHour) &&
        CheckNullable(Validity, other.Validity);
    }

    return base.Equals(obj);
  }

  /// <summary>
  /// Get the hash code of the object
  /// </summary>
  /// <returns></returns>
  public override int GetHashCode()
  {
    return base.GetHashCode();
  }

  private bool CheckNullable<T>(T objProps, T otherProps)
  {
    // if other is null, we don't compare the property
    if (otherProps == null)
      return true;

    return objProps != null && objProps.Equals(otherProps);
  }

  private bool CheckSequence<T>(List<T> objProps, List<T> otherProps)
  {
    // if other is null, we don't compare the property
    if (otherProps == null)
      return true;

    return objProps != null && objProps.SequenceEqual(otherProps);
  }
}
