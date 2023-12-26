using System;
using System.IO;
using System.Text;
using UnityEngine;

public class FileDataHandler
{
    private string _directoryPath = "";
    private string _fileName = "";

    //save by simple encryption
    private bool _encrypt = false;
    private string _codeWord = "ggm_high"; //test encryption code
    private CryotoModule _myCryptoModule;

    public FileDataHandler(string directoryPath, string fileName, bool encrypt)
    {
        _directoryPath = directoryPath;
        _fileName = fileName;
        _encrypt = encrypt;
        _myCryptoModule = new CryotoModule();
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(_directoryPath, _fileName);

        try
        {
            Directory.CreateDirectory(_directoryPath);
            string dataToStore = JsonUtility.ToJson(data, true);

            if (_encrypt)
            {
                dataToStore = _myCryptoModule.AESEncrypt256(dataToStore);
                //dataToStore = EncryptAndDeCryptData(dataToStore);
                //dataToStore = Base64Process(dataToStore, true);
            }
            //if you are use using statement then auto close filestream
            using (FileStream writeStream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(writeStream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error on trying to save data to file {fullPath} \n {ex.Message}");
        }
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(_directoryPath, _fileName);
        GameData loadedData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                //if you are use using statement then auto close filestream
                using (FileStream readStream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(readStream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                if (_encrypt)
                {
                    dataToLoad = _myCryptoModule.Decrypt(dataToLoad);
                    //dataToLoad = EncryptAndDeCryptData(dataToLoad);
                    //dataToLoad = Base64Process(dataToLoad, false); 
                }
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error on trying to load data to file {fullPath} \n");
                Debug.LogError(ex);
            }
        }

        return loadedData;
    }

    public void DeleteSaveData()
    {
        string fullPath = Path.Combine(_directoryPath, _fileName);

        if (File.Exists(fullPath))
        {
            try
            {
                File.Delete(fullPath);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error on trying to delete data file : {fullPath} \n {ex.Message}");
            }
        }
    }

    //simple xor encryption
    private string EncryptAndDeCryptData(string data)
    {
        StringBuilder sBuilder = new StringBuilder();

        for (int i = 0; i < data.Length; ++i)
        {
            sBuilder.Append((char)(data[i] ^ _codeWord[i % _codeWord.Length]));
        }

        return sBuilder.ToString();
    }

    private string Base64Process(string data, bool encoding)
    {
        if(encoding)
        {
            byte[] dataByteArr = Encoding.UTF8.GetBytes(data);
            return Convert.ToBase64String(dataByteArr);
        }else
        {
            byte[] dataByteArr = Convert.FromBase64String(data);
            return Encoding.UTF8.GetString(dataByteArr);
        }
    }

}
