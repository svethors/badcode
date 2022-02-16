using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Logging.Abstractions;

// Main
Person data = new Person()
{
	Id = Guid.NewGuid(),
	Name = "Name"
};

ILoggerFactory loggerFactory = new NullLoggerFactory();
ILogger logger = loggerFactory.CreateLogger("main");

IRepository<Person> repo = new Repository<Person>(loggerFactory);

try
{
	repo.Add(data).Wait();
}
catch(Exception ex)
{
	logger.LogError(ex.Message, ex);
}

Console.ReadLine();

// END Main

public class Person
{
	public Guid Id { get; set; }
	public string Name { get; set; } = "";
	public string IdName { get { return $"{Id} - {Name}"; } }
}

public interface IRepository<T>
{
	Task Add(T item);
}

public class Repository<T> : IRepository<T>, IDisposable
{
	private readonly ILogger _logger;

	private string _cstr = "server=localhost;catalog=logiflow;user=sa;pw=OIINDioeed98u93noinef98sdhf9nsdoifn34e0898j4rfd";
	private bool _disposedValue;

	public Repository(ILoggerFactory loggerFactory)
	{
		_logger = loggerFactory.CreateLogger<T>();
	}

	public Task Add(T item)
	{
		try
		{
			string query = "insert into " + typeof(T).Name + " (";
			string columns = "(";
			string values = "(";

			foreach(var pi in typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
			{
				columns += pi.Name + ", ";
				values += pi.GetValue(item)?.ToString() + ", ";
			}

			columns = columns[..(columns.Length - 2)] + ")";
			values = values[..(values.Length - 2)] + ")";

			query += columns + " values " + values;

			var conn = new SqlConnection(_cstr);
			return conn.ExecuteAsync(query);
		}
		catch (SqlException ex)
		{
			_logger.LogError(ex.Message, ex);
			throw ex;
		}
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposedValue)
		{
			if (disposing)
			{
			}
			_disposedValue = true;
		}
	}

	// // Override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
	// ~Repository()
	// {
	//     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
	//     Dispose(disposing: false);
	// }

	public void Dispose()
	{
		// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}