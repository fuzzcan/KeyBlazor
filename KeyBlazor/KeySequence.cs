namespace KeyBlazor
{
    public class KeySequence
    {
        private readonly List<string> _keys = new();

        public void Add(string key)
        {
            _keys.Add(key);
        }

        public void Clear()
        {
            _keys.Clear();
        }

        public bool IsMatching(HotKey hotKey)
        {
            if (_keys.Count < hotKey.Keys.Length)
                return false;

            return !_keys.Where((t, i) => t != hotKey.Keys[i]).Any();
        }

        public override string ToString()
        {
            return string.Join("+", _keys);
        }
    }
}
