namespace Fontys.BlockExplorer.API.Test.Controllers
{
    using Xunit;
    using FluentAssertions;

    public class BlockControllerTest
    {
        [Fact]
        public void Get_Block_Return_Excisting()
        {
            // arrange
            var hash = "Test Hash"; 

            // act

            // assert
            hash.Should().NotBeEmpty(); 
        }

    }
}
