using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;

public class Program
{
    static Dictionary<string, string> userData = new();
    public static void Main()
    {
        const string Menu = "\n PROJECTE DE SEGURETAT:" +
            "\n ---------------------" +
            "\n [1] Registre d'usuari" +
            "\n [2] Verificació de dades" +
            "\n [3] Encriptació i desencriptació RSA" +
            "\n [0] Sortir de l'aplicació";
        const string SelectOption = "\n\n Selecciona una acció: ";
        const string NotValid = "\n Aquesta acció no existeix";

        bool exit = false;
        string? chosenOption;

        while (!exit)
        {
            Console.WriteLine(Menu);
            Console.Write(SelectOption);
            chosenOption = Console.ReadLine();

            switch (chosenOption)
            {
                case "1":
                    RegisterUser();
                    break;
                case "2":
                    if (VerifyUserData())
                    {
                        Console.WriteLine("Dades correctes!");
                    }
                    else
                    {
                        Console.WriteLine("Dades incorrectes!");
                    }
                    break;
                case "3":
                    EncryptRSA();
                    break;
                case "0":
                    exit = true;
                    break;
                default:
                    Console.WriteLine(NotValid);
                    break;
            }
        }
    }
    public static void RegisterUser()
    {
        const string WriteName = "Introdueix el teu nom: ";
        const string WritePassword = "Introdueix la teva contrasenya: ";
        const string UserAlreadyReg = "Ja hi ha un usuari registrat!";

        Console.Write(WriteName);
        string? name = Console.ReadLine();

        Console.Write(WritePassword);
        string? password = Console.ReadLine();

        string hash = generateHash(name + password);
        userData[name] = hash;

        Console.WriteLine($"Usuari registrat. Combinació hash: {hash}");
    }
    public static bool VerifyUserData()
    {
        const string WriteName = "Introdueix el teu nom: ";
        const string WritePassword = "Introdueix la teva contrasenya: ";
        Console.Write(WriteName);
        string? name = Console.ReadLine();

        Console.Write(WritePassword);
        string? password = Console.ReadLine();

        string hash = generateHash(name + password);

        if (userData.ContainsKey(name) && userData[name] == hash)
        {
            return true;
        }
        return false;
    }
    public static string generateHash(string input)
    {
        string hashSTR;
        SHA256 hash = SHA256.Create();
        byte[] hashBytes = hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        hashSTR = BitConverter.ToString(hashBytes).Replace("-", "");
        hash.Clear();
        return hashSTR;
    }
    public static void EncryptRSA()
    {
        using (RSA rsa = RSA.Create())
        {
            // Genera clau pública i privada
            RSAParameters publicKey = rsa.ExportParameters(false);
            RSAParameters privateKey = rsa.ExportParameters(true);

            Console.Write("Introdueix un text per encriptar: ");
            string missatge = Console.ReadLine();

            // Encripta amb la clau pública
            byte[] textEncriptat = EncriptRSA(missatge, publicKey);
            Console.WriteLine($"Text encriptat: {Convert.ToBase64String(textEncriptat)}");

            // Desencripta amb la clau privada
            string textDesencriptat = DecryptRSA(textEncriptat, privateKey);
            Console.WriteLine($"Text desencriptat: {textDesencriptat}");
        }
    }
    public static byte[] EncriptRSA(string msg, RSAParameters publicKey)
    {
        using (RSA rsa = RSA.Create())
        {
            rsa.ImportParameters(publicKey);
            return rsa.Encrypt(Encoding.UTF8.GetBytes(msg), RSAEncryptionPadding.OaepSHA256);
        }
    }
    public static string DecryptRSA(byte[] encryptedMsg, RSAParameters privateKey)
    {
        using (RSA rsa = RSA.Create())
        {
            rsa.ImportParameters(privateKey);
            byte[] textDesencriptat = rsa.Decrypt(encryptedMsg, RSAEncryptionPadding.OaepSHA256);
            return Encoding.UTF8.GetString(textDesencriptat);
        }
    }
}