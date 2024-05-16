namespace SalaryManager.Domain.Repositories;

/// <summary>
/// Repository - PDF変換
/// </summary>
public interface IPDFConverterRepository
{
    List<string> ConvertPDFIntoImage(string path);
}
