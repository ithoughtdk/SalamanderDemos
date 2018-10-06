using System;
using Salamander;

namespace Ducklings
{
    public class Ducklings
    {
        private const long Pond1Id = 1L;
        private const long Pond2Id = 2L;

        public void Run()
        {
            var salamanderServer = new SalamanderServer();
            using (var database = salamanderServer
                .Create("Database.salamander", true))
            {
                Setup(database);
                DisplayStatus(database);
                TransferDucklings(database);
                DisplayStatus(database);
                Console.WriteLine("Press any key to continue");
                Console.ReadLine();
            }
        }
        private void DisplayStatus(IDatabase database)
        {
            using (var origin = database.Mount<Origin>())
            {
                var sourcePond = origin.Ponds.First(pond => pond.Id == Pond1Id);
                Console.WriteLine($"Pond #1 has {sourcePond.DucklingCount} ducklings");
                var destinationPond = origin.Ponds.First(pond => pond.Id == Pond2Id);
                Console.WriteLine($"Pond #2 has {destinationPond.DucklingCount} ducklings");
            }
        }

        // Create two ponds with ducklings
        private void Setup(IDatabase database)
        {
            using (var origin = database.Mount<Origin>())
            {
                var pond1 = origin.Ponds.Create(Pond1Id);
                pond1.Id = Pond1Id;
                pond1.DucklingCount = 20;
                var pond2 = origin.Ponds.Create(Pond2Id);
                pond2.Id = Pond2Id;
                pond2.DucklingCount = 10;
                origin.Commit();
            }
        }

        private void TransferDucklings(IDatabase database)
        {
            using (var origin = database.Mount<Origin>())
            {
                // An example of a child transaction
                using (var transferDucklings = origin.New<TransferDucklings>())
                {
                    var ducklingsToTransfer = 8;
                    transferDucklings.SourcePondId = Pond1Id;
                    transferDucklings.DestinationPondId = Pond2Id;
                    transferDucklings.Transfer(ducklingsToTransfer);
                    transferDucklings.Commit();
                    Console.WriteLine($"Transferred {ducklingsToTransfer} ducklings from pond #1 to pond #2");
                }
                origin.Commit();
            }
        }
    }

    public sealed partial class TransferDucklings : Nature
    {
        public void Transfer(int numberOfDucklings)
        {
            VerifyDucklingAvailability(numberOfDucklings);
            _source.DucklingCount -= numberOfDucklings;
            _destination.DucklingCount += numberOfDucklings;
        }

        private void VerifyDucklingAvailability(decimal numberOfDucklings)
        {
            if (_source.DucklingCount < numberOfDucklings)
                throw new InsufficientDucklingsException();
        }
    }

    public sealed class InsufficientDucklingsException : Exception
    {
    }
}
