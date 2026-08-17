using System.Text;

public class AlrRegen
{
    public static void AlrRegenerate(string inputDir, string outputDir, ProgressBar progressBar)
    {
        string inputDirectory = inputDir;
        string outputDirectory = outputDir;
        RegenerateAll(inputDirectory, outputDirectory, progressBar);
    }

    public static void RegenerateAll(string inputDirectory, string outputDirectory, ProgressBar progressBar)
    {
        if (!Directory.Exists(inputDirectory))
        {
            Console.WriteLine($"ERROR: Input folder '{inputDirectory}' does not exist.");
            return;

        }
        //Read both _alr and script files from the input directory
        List<string> alrFileNames = Directory.GetFiles(inputDirectory)
                .Where(f => Path.GetFileName(f).Contains("_alr", StringComparison.OrdinalIgnoreCase))
                .ToList();
        List<string> scriptFileNames = Directory.GetFiles(inputDirectory)
                .Where(f => !Path.GetFileName(f).Contains("_alr", StringComparison.OrdinalIgnoreCase))
                .ToList();

        //Create lists to hold the files
        List<AlrFile> alrFiles = new List<AlrFile>();
        List<ScriptFile> scriptFiles = new List<ScriptFile>();

        if (alrFileNames.Count == 0)
        {
            Console.WriteLine("No _alr files found");
            return;
        }
        else
        {
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }
            foreach (var file in alrFileNames)
            {
                alrFiles.Add(ReadAlrFile(file));
            }
            foreach (var file in scriptFileNames)
            {
                scriptFiles.Add(ReadScriptFile(file));
            }
            // Sort alrFiles by StartingFlag, with nulls first
            alrFiles = alrFiles
                .OrderBy(f => f.StartingFlag.HasValue ? 1 : 0) // nulls first
                .ThenBy(f => f.StartingFlag)
                .ToList();
            int position = 0;
            foreach (var alrFile in alrFiles)
            {
                ScriptFile? matchingScript = scriptFiles.FirstOrDefault(s => string.Equals(s.ScriptTitle, alrFile.ScriptTitle.Substring(0, alrFile.ScriptTitle.Length - 4), StringComparison.OrdinalIgnoreCase));
                if (matchingScript != null)
                {
                    byte[] alr = RegenerateAlrFile(alrFile, matchingScript, ref position);
                    File.WriteAllBytes(Path.Combine(outputDirectory, alrFile.FileName), alr);
                }
                else
                {
                    Console.WriteLine($"No matching script file found for alr: {alrFile.ScriptTitle}");
                }
                progressBar.Value = (int)((double)alrFiles.IndexOf(alrFile) / alrFiles.Count * 100);
            }
            
            Console.WriteLine($"Found {alrFiles.Count} _alr files in {inputDirectory}");
        }
    }

    public static AlrFile ReadAlrFile(string filePath)
    {
        byte[] data = File.ReadAllBytes(filePath);

        int offset = 0;
        int nameLen = BitConverter.ToInt32(data, offset);
        offset += 4;
        if (nameLen < 0 || data.Length < offset + nameLen) { 
            throw new InvalidDataException("Invalid file name length in _alr file.");
        }
        string scriptFileName = Encoding.UTF8.GetString(data, offset, nameLen);
        offset += nameLen;
        int size = BitConverter.ToInt32(data, offset);
        offset += 4;
        offset = RoundUpToNextMultipleOf4(offset);
        int uabeaOffset = offset;

        var entries = new List<KeyValuePair>();
        if (size > 0)
        {
            while (offset + 8 <= data.Length)
            {
                int key = BitConverter.ToInt32(data, offset);
                int value = BitConverter.ToInt32(data, offset + 4);
                entries.Add(new KeyValuePair { Key = key, Value = value });
                offset += 8;
            }
        }

        return new AlrFile
        {
            FileName = Path.GetFileName(filePath),
            ScriptTitleLength = nameLen,
            ScriptTitle = scriptFileName,
            DataSize = size,
            UabeaHeaderSize = uabeaOffset,
            StartingFlag =  entries.Count > 0 ? entries[0].Value : null,
            Entries = entries
        };
    }

    public static ScriptFile ReadScriptFile(string filePath)
    {
        string allScriptText = File.ReadAllText(filePath, Encoding.Unicode);
        byte[] data = Encoding.Unicode.GetBytes(allScriptText);

        int offset = 0;
        int nameLen = BitConverter.ToInt32(data, offset);
        offset += 4;
        if (nameLen < 0 || data.Length < offset + nameLen)
        {
            throw new InvalidDataException("Invalid file name length in _alr file.");
        }
        string scriptFileName = Encoding.UTF8.GetString(data, offset, nameLen);
        offset += nameLen;
        offset = RoundUpToNextMultipleOf4(offset);
        int size = BitConverter.ToInt32(data, offset);
        offset += 4;
        if (data[offset] == 255 && data[offset + 1] == 254) {
            offset += 2;
        }
        int uabeaOffset = offset;

        byte[] scriptText = data.Skip(uabeaOffset).ToArray();

        return new ScriptFile
        {
            FileName = Path.GetFileName(filePath),
            ScriptTitleLength = nameLen,
            ScriptTitle = scriptFileName,
            DataSize = size,
            UabeaHeaderSize = uabeaOffset,
            ScriptText = scriptText,
        };
    }

    public static byte[] RegenerateAlrFile(AlrFile alr, ScriptFile script, ref int position)
    {
        List<byte> newAlr = new List<byte>();

        newAlr.AddRange(BitConverter.GetBytes(alr.ScriptTitleLength));
        newAlr.AddRange(Encoding.UTF8.GetBytes(alr.ScriptTitle));

        if (alr.StartingFlag == null) 
        {
            newAlr.AddRange(BitConverter.GetBytes(0));
            int padding = (4 - (newAlr.Count % 4)) % 4;
            for (int i = 0; i < padding; i++)
            {
                newAlr.Add(0x00);
            }
            return newAlr.ToArray();
        }
        else
        {
            //Padding for title
            int padding = (4 - (newAlr.Count % 4)) % 4;
            for (int i = 0; i < padding; i++)
            {
                newAlr.Add(0x00);
            }
        }
        List<string> searchStrings = new List<string>() {
        "＠",
        "<voice name=",
        "<textboxtype type=\"monologue\">",
        "<textboxtype type=\"monologue2\">",
        "<textboxtype type=\"monologue3\">",
        "<monologue background=\"on\">",
        };
        List<(string, int)> stringPositions = FindStringPositions(script.ScriptText);
        List<(string, int)> filteredStringPositions = stringPositions
            .Where(pos => searchStrings.Any(s => pos.Item1.Contains(s)))
            .ToList();
        //Add length of data
        newAlr.AddRange(BitConverter.GetBytes(filteredStringPositions.Count * 8));

        for (int i = 0; i < stringPositions.Count; i++)
        {
            if (searchStrings.Any(s => stringPositions[i].Item1.Contains(s)))
            {
                newAlr.AddRange(BitConverter.GetBytes(stringPositions[i].Item2));
                newAlr.AddRange(BitConverter.GetBytes(position));
                var isNormalTag = searchStrings.Any(t => stringPositions[i].Item1.Contains(t));
                if (stringPositions[i].Item1.Contains("<voice name="))
                {
                    continue;
                }
                else if (isNormalTag)
                {
                    position += 1;
                }
            }
        }
        return newAlr.ToArray();
    }

    public static List<(string SearchString, int Position)> FindStringPositions(byte[] scriptBytes)
    {
        var results = new List<(string, int)>();
        string scriptText = Encoding.Unicode.GetString(scriptBytes);

        // 1. Find all <...> tags not commented out
        var tagRegex = new System.Text.RegularExpressions.Regex(@"<[^>\r\n]+>");
        foreach (System.Text.RegularExpressions.Match match in tagRegex.Matches(scriptText))
        {
            if (match.Value == "<pb>") continue; // Skip <pb> tags

            int lineStart = scriptText.LastIndexOf('\n', match.Index);
            if (lineStart == -1) lineStart = 0; else lineStart++;
            int lineEnd = scriptText.IndexOf('\n', match.Index);
            if (lineEnd == -1) lineEnd = scriptText.Length;
            string line = scriptText.Substring(lineStart, lineEnd - lineStart).TrimStart();

            if (!line.StartsWith("//"))
            {
                int byteIndex = Encoding.Unicode.GetByteCount(scriptText.Substring(0, match.Index));
                results.Add((match.Value, byteIndex));
            }
        }

        // 2. Find all lines that start with ＠ and are not commented out
        int currentIndex = 0;
        foreach (var line in scriptText.Split('\n'))
        {
            string trimmedLine = line.TrimStart();
            if (!trimmedLine.StartsWith("//") && trimmedLine.StartsWith("＠"))
            {
                int byteIndex = Encoding.Unicode.GetByteCount(scriptText.Substring(0, currentIndex));
                results.Add((line, byteIndex));
            }
            currentIndex += line.Length + 1; // +1 for '\n'
        }
        return results.OrderBy(r => r.Item2).ToList();
    }

    public static int RoundUpToNextMultipleOf4(int number)
    {
        return (number % 4 == 0) ? number : number + (4 - (number % 4));
    }

    public class AlrFile
    {
        public string? FileName;
        public int ScriptTitleLength;
        public string? ScriptTitle;
        public int DataSize;
        public int UabeaHeaderSize;
        public int? StartingFlag;
        public List<KeyValuePair> Entries = new List<KeyValuePair>();
    }

    public class ScriptFile
    {
        public string? FileName;
        public int ScriptTitleLength;
        public string? ScriptTitle;
        public int DataSize;
        public int? UabeaHeaderSize;
        public byte[]? ScriptText;
    }

    public struct KeyValuePair
    {
        public int Key;
        public int Value;
    }
}