﻿using System.Linq;
using Xunit;

namespace Versionize.Tests
{
    public class ConventionalCommitParserTests
    {
        [Fact]
        public void ShouldParseTypeScopeAndSubjectFromSingleLineCommitMessage()
        {
            var parser = new ConventionalCommitParser();

            var testCommit = new TestCommit("feat(scope): broadcast $destroy event on scope destruction");
            var conventionalCommit = parser.Parse(testCommit);

            Assert.Equal("feat", conventionalCommit.Type);
            Assert.Equal("scope", conventionalCommit.Scope);
            Assert.Equal("broadcast $destroy event on scope destruction", conventionalCommit.Subject);
        }

        [Fact]
        public void ShouldUseFullHeaderAsSubjectIfNoTypeWasGiven()
        {
            var parser = new ConventionalCommitParser();

            var testCommit = new TestCommit("broadcast $destroy event on scope destruction");
            var conventionalCommit = parser.Parse(testCommit);

            Assert.Equal(testCommit.Message, conventionalCommit.Subject);
        }

        [Fact]
        public void ShouldUseFullHeaderAsSubjectIfNoTypeWasGivenButSubjectUsesColon()
        {
            var parser = new ConventionalCommitParser();

            var testCommit = new TestCommit("broadcast $destroy event: on scope destruction");
            var conventionalCommit = parser.Parse(testCommit);

            Assert.Equal(testCommit.Message, conventionalCommit.Subject);
        }

        [Fact]
        public void ShouldParseTypeScopeAndSubjectFromSingleLineCommitMessageIfSubjectUsesColon()
        {
            var parser = new ConventionalCommitParser();

            var testCommit = new TestCommit("feat(scope): broadcast $destroy: event on scope destruction");
            var conventionalCommit = parser.Parse(testCommit);

            Assert.Equal("feat", conventionalCommit.Type);
            Assert.Equal("scope", conventionalCommit.Scope);
            Assert.Equal("broadcast $destroy: event on scope destruction", conventionalCommit.Subject);
        }

        [Fact]
        public void ShouldExtractCommitNotes()
        {
            var parser = new ConventionalCommitParser();

            var testCommit = new TestCommit("feat(scope): broadcast $destroy: event on scope destruction\nBREAKING CHANGE: this will break rc1 compatibility");
            var conventionalCommit = parser.Parse(testCommit);

            Assert.Single(conventionalCommit.Notes);

            var breakingChangeNote = conventionalCommit.Notes.Single();

            Assert.Equal("BREAKING CHANGE", breakingChangeNote.Title);
            Assert.Equal("this will break rc1 compatibility", breakingChangeNote.Text);
        }
    }
}
