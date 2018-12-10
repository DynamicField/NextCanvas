using System.Text;

namespace NextCanvas
{
    public class LogModel
    {
        private StringBuilder _builder = new StringBuilder();

        public void SetBuilder(StringBuilder b)
        {
            _builder = b;
        }

        public string Log
        {
            get => _builder.ToString();
            set => _builder = new StringBuilder(value);
        }
    }
}
