using Asys.Mocks.Tasks.Process;
using Asys.Tasks.Process;
using Asys.Tasks.Process.Context;
using Asys.Tests.Framework;
using Asys.Tests.Framework.Asserts;
using Asys.Tests.Framework.Logger;
using Moq;
using Moq.Language;

namespace ChocolateySharp.Tests.Unit
{
    internal abstract class ChocolateyClientTestData<TChocolatetyClientTestData> : ITestData
        where TChocolatetyClientTestData : ChocolateyClientTestData<TChocolatetyClientTestData>
    {
        public ChocolateyClientTestData(ITestLogger logger)
        {
            MockValildatenstalledChocoProcessTask = new Mock<ProcessTask>();
            MockInstallChocoProcessTask = new Mock<ProcessTask>();
            MockChocoProcessTasks = new List<Mock<ProcessTask>>();

            var mockProcessTaskFactory = new Mock<IProcessTaskFactory>();
            MockProcessTaskFactoryCreateSequence = mockProcessTaskFactory.SetupSequence(m => m.Create());

            Client = new ChocolateyClient(logger, mockProcessTaskFactory.Object);
        }

        private IChocolateyClient Client { get; }

        private ISetupSequentialResult<ProcessTask> MockProcessTaskFactoryCreateSequence { get; }

        private Mock<ProcessTask> MockValildatenstalledChocoProcessTask { get; }

        private Mock<ProcessTask> MockInstallChocoProcessTask { get; }

        private  IList<Mock<ProcessTask>> MockChocoProcessTasks { get; }

        private Exception? Exception { get; set; }

        protected abstract Task InternalActAsync(IChocolateyClient client, ActOptions? actOptions = null);

        public async Task ActAsync(ActOptions? actOptions = null)
        {
            try
            {
                await InternalActAsync(Client, actOptions);
            }
            catch (Exception e)
            {
                Exception = e;
            }
        }

        public TChocolatetyClientTestData WithValidateInstalledChoco()
        {
            MockProcessTaskFactoryCreateSequence.Returns(() => MockValildatenstalledChocoProcessTask.Object);
            MockValildatenstalledChocoProcessTask.WithSuccessfulResults();
            return (TChocolatetyClientTestData)this;
        }

        public TChocolatetyClientTestData ButNoInstalledChocoFound()
        {
            MockValildatenstalledChocoProcessTask.WithFailureResults();
            return (TChocolatetyClientTestData)this;
        }

        public TChocolatetyClientTestData WithInstallChoco()
        {
            MockProcessTaskFactoryCreateSequence.Returns(() => MockInstallChocoProcessTask.Object);
            MockInstallChocoProcessTask.WithSuccessfulResults();
            return (TChocolatetyClientTestData)this;
        }

        public TChocolatetyClientTestData ButFailedToInstallChoco()
        {
            MockInstallChocoProcessTask.WithFailureResults();
            return (TChocolatetyClientTestData)this;
        }

        public TChocolatetyClientTestData WithExecuteChocoCommand(int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                var mockChocoProcessTask = new Mock<ProcessTask>().WithSuccessfulResults();
                MockProcessTaskFactoryCreateSequence.Returns(() => mockChocoProcessTask.Object);
                MockChocoProcessTasks.Add(mockChocoProcessTask);
            }

            return (TChocolatetyClientTestData)this;
        }

        public TChocolatetyClientTestData AndFailChocoCommand(int index = 0)
        {
            MockChocoProcessTasks[index]
                .WithFailureResults();
            return (TChocolatetyClientTestData)this;
        }

        public TChocolatetyClientTestData HadException(Exception exception)
        {
            Assert.NotNull(Exception);
            Assert.IsType(exception.GetType(), Exception);
            return (TChocolatetyClientTestData)this;
        }

        public TChocolatetyClientTestData HadException<TException>()
            where TException : Exception, new()
            => HadException(new TException());

        public TChocolatetyClientTestData HadNoException()
        {
            Assert.Null(Exception);
            return (TChocolatetyClientTestData)this;
        }

        public TChocolatetyClientTestData VerifyValidationForInstalledChocoOccurred(int times = 1)
        {
            MockValildatenstalledChocoProcessTask.Verify(m => m.ExecuteAsync(It.IsAny<ProcessTaskContext>()), Times.Exactly(times));
            return (TChocolatetyClientTestData)this;
        }

        public TChocolatetyClientTestData VerifyInstallationOfChocoOccurred(int times = 1)
        {
            MockInstallChocoProcessTask.Verify(m => m.ExecuteAsync(It.IsAny<ProcessTaskContext>()), Times.Exactly(times));
            return (TChocolatetyClientTestData)this;
        }

        public TChocolatetyClientTestData VerifyExecutionOfChocoCommandOccurred(int times = 1)
        {
            Assert.Equal(times, MockChocoProcessTasks.Count);
            MockChocoProcessTasks.ToList().ForEach(m => m.Verify(m => m.ExecuteAsync(It.IsAny<ProcessTaskContext>()), Times.Once));
            return (TChocolatetyClientTestData)this;
        }

        public void Dispose() { }
    }
}
