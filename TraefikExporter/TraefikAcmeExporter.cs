using System.Security.Cryptography.X509Certificates;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TraefikAcmeExport;

public class TraefikAcmeExporter
{
    public void ExportAcmeFile(string acmeJsonContent, string outPath)
    {
        JObject acmeJson = JObject.Parse(acmeJsonContent);
        JObject letsencrypthttp = (JObject)acmeJson["letsencrypthttp"];
        JArray certificates = (JArray)letsencrypthttp["Certificates"];

        foreach (var letsEncryptCert in certificates)
        {
            string domain = (string)letsEncryptCert["domain"]["main"];
            string privateKey = (string)letsEncryptCert["key"];
            string certificate = (string)letsEncryptCert["certificate"];

            if (!IsCertValid(certificate))
            {
                Console.WriteLine("The cert for '{0}' is invalid", domain);
                continue;
            }

            var domainPath = Path.Join(outPath, domain);
            Directory.CreateDirectory(domainPath);

            Console.WriteLine("Exporting {0} to {1}", domain, domainPath);
            SaveFileWithoutBOM($"{domainPath}/key.pem", Base64ToString(privateKey));
            SaveFileWithoutBOM($"{domainPath}/cert.pem", Base64ToString(certificate));
        }

        Console.WriteLine("Certificate files exported successfully.");
    }

    private bool IsCertValid(string cert)
    {
        try
        {
            using X509Certificate2 domainCert = new X509Certificate2(Convert.FromBase64String(cert));
            using X509Chain chain = new X509Chain();
            chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
            bool isValid = chain.Build(domainCert);
            return isValid;
        }
        catch (Exception e)
        {
            Console.WriteLine();
            return false;
        }
    }

    private string Base64ToString(string base64)
    {
        byte[] byteArray = Convert.FromBase64String(base64);
        return Encoding.UTF8.GetString(byteArray);
    }

    private void SaveFileWithoutBOM(string filePath, string content)
    {
        try
        {
            using StreamWriter writer = new StreamWriter(filePath, false, new UTF8Encoding(false));
            writer.Write(content);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving the file: {ex.Message}");
        }
    }
}