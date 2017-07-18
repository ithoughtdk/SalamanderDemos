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
        private Directory<PondOf, Pond> _ponds;

        public INodeView<PondOf, Pond> Ponds
        {
            get { return _ponds.AsNodeView<PondOf, Pond>(); }
        }

        protected override void OnMount()
        {
            _ponds = Mount<DirectoryOf<Directory<PondOf, Pond>>,
                Directory<PondOf, Pond>>();
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
                _source = Ponds.First((r, pond) => pond.Id == value);
            }
        }

        public long DestinationPondId
        {
            set
            {
                _destination = Ponds.First((r, pond) => pond.Id == value);
            }
        }

        private Pond _source;
        private Pond _destination;
    }
}
