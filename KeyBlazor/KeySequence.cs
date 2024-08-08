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

        public bool IsMatching(Hotkey hotkey)
        {
            if (_keys.Count < hotkey.Keys.Length)
                return false;

            return !_keys.Where((t, i) => t != hotkey.Keys[i]).Any();
        }

        public override string ToString()
        {
            return string.Join("+", _keys);
        }
    }
}
