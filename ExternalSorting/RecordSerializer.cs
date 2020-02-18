namespace ExternalSorting
{
    public static class RecordSerializer
    {
        public static string Serialize(in Record record)
        {
            return $"{record.Number}. {record.Sentence}";
        }

        public static Record Deserialize(string str)
        {
            var parts = str.Split(".");
            return new Record(int.Parse(parts[0]), parts[1].TrimStart());
        }
    }
}