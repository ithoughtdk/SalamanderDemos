// This is a generated file
using Salamander;

namespace Ducklings
{
    public sealed partial class PondOf : Relation
    {
    }

    public sealed partial class Pond : IdNode<long>
    {
        private Property<int> _ducklingCount;

        public int DucklingCount
        {
            get { return _ducklingCount.Value; }
            set { _ducklingCount.Value = value; }
        }

        protected override void OnMount()
        {
            base.OnMount();
            _ducklingCount = Mount<int>(nameof(_ducklingCount));
        }
    }

    public abstract partial class Nature : Transaction
    {
        public IDirectory<Pond, long> Ponds { get; private set; }

        protected override void OnMount()
        {
            Ponds = Directory<Pond, long>();
        }
    }

    public sealed partial class Origin : Nature
    {
    }

    public sealed partial class TransferDucklings : Nature
    {
        public long SourcePondId
        {
            set
            {
                _source = Ponds.First(node => node.Id == value);
            }
        }

        public long DestinationPondId
        {
            set
            {
                _destination = Ponds.First(node => node.Id == value);
            }
        }

        private Pond _source;
        private Pond _destination;
    }
}