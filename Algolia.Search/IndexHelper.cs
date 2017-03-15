using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Algolia.Search
{
    /// <summary>
    /// Helper class for making it simpler to work with an index.
    /// </summary>
    /// <typeparam name="T">The type of data for the index.</typeparam>
    public class IndexHelper<T> : Index
    {
        private readonly string _indexName;
        private readonly string _objectIdField;

        /// <summary>
        /// Create a new IndexHelper.
        /// </summary>
        /// <param name="client">The AlgoliaClient to use for index management.</param>
        /// <param name="indexName">The name of the Algolia index.</param>
        /// <param name="objectIdField">The name of the field to use for mapping to the Algolia objectID.</param>
        public IndexHelper(AlgoliaClient client, string indexName, string objectIdField = "Id")
            : base(client, indexName)
        {
            // Save
            _client = client;
            _indexName = indexName;
            _objectIdField = objectIdField;
        }

        /// <summary>
        /// Indexes all objects using a temporary index that is then atomically moved to the destination index.
        /// All previous content will be overwritten. You must supply all objects to this method that need to
        /// exist within the index.
        /// </summary>
        /// <param name="objects">An enumerable list of objects to override the index with.</param>
        /// <param name="maxObjectsPerCall">Maximum number of objects per indexing call. Should be between 1,000 and 10,000 depending on size of objects.</param>
        async public Task<JObject> OverwriteIndexAsync(IEnumerable<T> objects, int maxObjectsPerCall = 1000)
        {
            // Build list for Tasks
            var taskList = new List<Task<JObject>>();

            //Index names
            var tempIndexName = _indexName + "_temp";

            // Use the temp index
            var tempIndex = _client.InitIndex(tempIndexName);

            // Setup array to store objects to index
            var toIndex = new List<JObject>();

            // Process each object
            foreach (var obj in objects)
            {
                // Convert obj to a JObject
                var jObject = JObject.FromObject(obj);

                // Get value used for objectID
                var id = jObject.GetValue(_objectIdField).ToString();

                // Override Algolia object ID with the object Id
                jObject.Add("objectID", id);

                // Save object for indexing
                toIndex.Add(jObject);

                // See if we have reached our limit
                if (toIndex.Count >= maxObjectsPerCall)
                {
                    // Add or update indices
                    taskList.Add(tempIndex.SaveObjectsAsync(toIndex));

                    // Reset array
                    toIndex.Clear();
                }
            }

            // Add or update indices for last batch
            if (toIndex.Count > 0)
                taskList.Add(tempIndex.SaveObjectsAsync(toIndex));

            // Wait for all tasks to be done
            Task.WaitAll(taskList.ToArray());

            // Overwrite main index with temp index
            return await _client.MoveIndexAsync(tempIndexName, _indexName);
        }

        /// <summary>
        /// Indexes all objects using a temporary index that is then atomically moved to the destination index.
        /// All previous content will be overwritten. You must supply all objects to this method that need to
        /// exist within the index.
        /// </summary>
        /// <param name="objects">An enumerable list of objects to override the index with.</param>
        /// <param name="maxObjectsPerCall">Maximum number of objects per indexing call. Should be between 1,000 and 10,000 depending on size of objects.</param>
        public JObject OverwriteIndex(IEnumerable<T> objects, int maxObjectsPerCall = 1000)
        {
            return OverwriteIndexAsync(objects, maxObjectsPerCall).Result;
        }

        /// <summary>
        /// Add or update the contents of several objects.
        /// </summary>
        /// <param name="objects">An enumerable list of objects to add or update.</param>
        /// <param name="maxObjectsPerCall">Maximum number of objects per indexing call. Should be between 1,000 and 10,000 depending on size of objects.</param>
        /// <returns>An array of objects containing an "objectIDs" attribute (array of string).</returns>
        async public Task<JObject[]> SaveObjectsAsync(IEnumerable<T> objects, int maxObjectsPerCall = 1000)
        {
            // Build list for Tasks
            var taskList = new List<Task<JObject>>();

            // Setup array to store objects to index
            var toIndex = new List<JObject>();

            // Process each object
            foreach (var obj in objects)
            {
                // Convert obj to a JObject
                var jObject = JObject.FromObject(obj);

                // Get value used for objectID
                var id = jObject.GetValue(_objectIdField).ToString();

                // Override Algolia object ID with the object Id
                jObject.Add("objectID", id);

                // Save object for indexing
                toIndex.Add(jObject);

                // See if we have reached our limit
                if (toIndex.Count >= maxObjectsPerCall)
                {
                    // Add or update indices
                    taskList.Add(base.SaveObjectsAsync(toIndex));

                    // Reset array
                    toIndex.Clear();
                }
            }

            // Add or update indices for last batch
            if (toIndex.Count > 0)
                taskList.Add(base.SaveObjectsAsync(toIndex));

            // Wait for all tasks to be done
            return await Task.WhenAll(taskList);
        }

        /// <summary>
        /// Add or update the contents of several objects.
        /// </summary>
        /// <param name="objects">An enumerable list of objects to add or update.</param>
        /// <param name="maxObjectsPerCall">Maximum number of objects per indexing call. Should be between 1,000 and 10,000 depending on size of objects.</param>
        /// <returns>An array of objects containing an "objectIDs" attribute (array of string).</returns>
        public JObject[] SaveObjects(IEnumerable<T> objects, int maxObjectsPerCall = 1000)
        {
            return SaveObjectsAsync(objects, maxObjectsPerCall).Result;
        }

        /// <summary>
        /// Add or update the contents of an object.
        /// </summary>
        /// <param name="obj">The object to add or update.</param>
        /// <returns>An object containing an "updatedAt" attribute.</returns>
        public Task<JObject> SaveObjectAsync(T obj)
        {
            // Convert obj to a JObject
            var jObject = JObject.FromObject(obj);

            // Get value used for objectID
            var id = jObject.GetValue(_objectIdField).ToString();

            // Override Algolia object ID with the object Id
            jObject.Add("objectID", id);

            // Add new index (if no matching jObject.objectID) or update
            return base.SaveObjectAsync(jObject);
        }

        /// <summary>
        /// Add or update the contents of an object.
        /// </summary>
        /// <param name="obj">The object to add or update.</param>
        /// <returns>An object containing an "updatedAt" attribute.</returns>
        public JObject SaveObject(T obj)
        {
            return SaveObjectAsync(obj).Result;
        }

        /// <summary>
        /// Delete several objects.
        /// </summary>
        /// <param name="objects">An enumerable list of objects to delete.</param>
        /// <param name="maxObjectsPerCall">Maximum number of objects per indexing call. Should be between 1,000 and 10,000 depending on size of objects.</param>
        /// <returns>An array of objects containing an "objectIDs" attribute (array of string).</returns>
        async public Task<JObject[]> DeleteObjectsAsync(IEnumerable<T> objects, int maxObjectsPerCall = 1000)
        {
            // Build list for Tasks
            var taskList = new List<Task<JObject>>();

            // Setup array to store objects to index
            var toIndex = new List<string>();

            // Process each object
            foreach (var obj in objects)
            {
                // Convert obj to a JObject
                var jObject = JObject.FromObject(obj);

                // Get value used for objectID
                var id = jObject.GetValue(_objectIdField).ToString();

                // Save object for indexing
                toIndex.Add(id);

                // See if we have reached our limit
                if (toIndex.Count >= maxObjectsPerCall)
                {
                    // Delete indices
                    taskList.Add(base.DeleteObjectsAsync(toIndex));

                    // Reset array
                    toIndex.Clear();
                }
            }

            // Delete indices for last batch
            if (toIndex.Count > 0)
                taskList.Add(base.DeleteObjectsAsync(toIndex));

            // Wait for all tasks to be done
            return await Task.WhenAll(taskList);
        }

        /// <summary>
        /// Delete several objects.
        /// </summary>
        /// <param name="objects">An enumerable list of objects to delete.</param>
        /// <param name="maxObjectsPerCall">Maximum number of objects per indexing call. Should be between 1,000 and 10,000 depending on size of objects.</param>
        /// <returns>An array of objects containing an "objectIDs" attribute (array of string).</returns>
        public JObject[] DeleteObjects(IEnumerable<T> objects, int maxObjectsPerCall = 1000)
        {
            return DeleteObjectsAsync(objects, maxObjectsPerCall).Result;
        }

        /// <summary>
        /// Delete an object from the index.
        /// </summary>
        /// <param name="obj">The object to delete.</param>
        /// <returns>An object containing a "deletedAt" attribute.</returns>
        public Task<JObject> DeleteObjectAsync(T obj)
        {
            // Convert obj to a JObject
            var jObject = JObject.FromObject(obj);

            // Get value used for objectID
            var id = jObject.GetValue(_objectIdField).ToString();

            // Remove the index from Algolia
            return base.DeleteObjectAsync(id);
        }

        /// <summary>
        /// Delete an object from the index.
        /// </summary>
        /// <param name="obj">The object to delete.</param>
        /// <returns>An object containing a "deletedAt" attribute.</returns>
        public JObject DeleteObject(T obj)
        {
            return DeleteObjectAsync(obj).Result;
        }

        /// <summary>
        /// The Algolia client
        /// </summary>
        public AlgoliaClient Client
        {
            get { return _client; }
        }

        /// <summary>
        /// The name of the index for this manager
        /// </summary>
        public string IndexName
        {
            get { return _indexName; }
        }

        /// <summary>
        /// The field of objects that maps to the Algolia objectID
        /// </summary>
        public string ObjectIdField
        {
            get { return _objectIdField; }
        }
    }
}