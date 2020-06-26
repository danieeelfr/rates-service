using Xunit;

namespace Services.Tests
{
    public class LoginTests
    {
        private const string EMAIL_WITH_MAX_SIZE_LIMIT_EXCEDEED = "validformatbutwithmorethan200charactersvalidformatbutwithmorethan200charactersvalidsformatbutwithmorethan200charactersvalidformatbutwithmorethan200charactersvalidformatbutwithmovalidformatbutwithmo.com";
        private const string PASSWORD_WITH_MAX_SIZE_LIMIT_EXCEDEED = "comvalidcomformatbutwicomthmorethan200charactersvalidformatbutwithmorethan200charactersvalidsformatbutwithmorethan200charactersvalidformatbutwithmorethan200charactersvalidformatbutwithmovalidformatbutwithmocom";
        private const string VALID_EMAIL = "valid-user@gmail.com";
        private const string VALID_PASSWORD = "pwd";
        private const string SECRET = "Xp2s5u8x/A?D(G+K";
        private readonly ILoginService _service;

        public LoginTests()
        {
            _service = new LoginService();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("invalid.com")]
        [InlineData(EMAIL_WITH_MAX_SIZE_LIMIT_EXCEDEED)]
        [InlineData("@invalid.com")]
        [InlineData("a@i.c")]
        [InlineData("invalid!user@gmail.com")]
        public void InvalidEmailShouldThrowException(string email)
        {
            var filter = buildFilter(email, VALID_PASSWORD);
            Assert.ThrowsAsync<BusinessException>(async () => await _service.Login(filter, SECRET)).Wait();
        }

        [Fact]
        public void InvalidPasswordShouldThrowException()
        {
            var filter = buildFilter(VALID_EMAIL, "");
            Assert.ThrowsAsync<BusinessException>(async () => await _service.Login(filter, SECRET)).Wait();
        }

        [Fact]
        public void validUserAndPasswordShouldReturnToken()
        {
            var filter = buildFilter(VALID_EMAIL, VALID_PASSWORD);
            var task = _service.Login(filter, SECRET);
            task.Wait();
            var result = task.Result;
            Assert.NotNull(result.AccessToken);
        }

        private static UserLoginFilter buildFilter(string email, string password)
        {
            return new UserLoginFilter()
            {
                Email = email,
                Password = password
            };
        }
    }
}
