using System.Text.Json;

namespace Entities.ErrorModel;

public class ErrorDetails<T>
{
	public bool Success { get; set; }
	public string Message { get; set; } = string.Empty;
	public T? Data { get; set; }
}
