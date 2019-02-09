using LibGit2Sharp;

namespace Versionize.Tests
{
    public class TestCommit : Commit
    {
        private readonly string _message;

        public TestCommit(string message)
        {
            _message = message;
        }

        public override string Message { get => _message; }
    }
}
