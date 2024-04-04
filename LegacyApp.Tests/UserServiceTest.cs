namespace LegacyApp.Tests;

public class UserServiceTest
{
    private readonly UserService _userService;
    private readonly UserTest _userTest;

    public UserServiceTest()
    {
        _userService = new UserService();
        _userTest = new UserTest
            {
                Client = 1,
                DateOfBirth = DateTime.Parse("1982-03-21"),
                EmailAddress = "johndoe@gmail.com",
                FirstName = "John",
                LastName = "Doe"
            };
    }
    
    [Fact]
    public void AddUser_Should_Return_When_FirstName_Is_Incorrect()
    {
        _userTest.FirstName = "";

        var userService = new UserService();
        var addResult = userService.AddUser(_userTest.FirstName,_userTest.LastName,_userTest.EmailAddress,_userTest.DateOfBirth, (int)_userTest.Client);
        Assert.False(addResult);
    }

    [Fact]
    public void AddUser_Should_Return_When_Email_Is_Incorrect()
    {
        _userTest.EmailAddress = "johndoegmailcom";

        var userService = new UserService();
        var addResult = userService.AddUser(_userTest.FirstName,_userTest.LastName,_userTest.EmailAddress,_userTest.DateOfBirth, (int)_userTest.Client);
        Assert.False(addResult);
    }

    [Fact]
    public void AddUser_Should_Return_When_Age_Incorrect()
    {
        _userTest.DateOfBirth = DateTime.Now;

        var userService = new UserService();
        var addResult = userService.AddUser(_userTest.FirstName,_userTest.LastName,_userTest.EmailAddress,_userTest.DateOfBirth, (int)_userTest.Client);
        Assert.False(addResult);
    }

    [Fact]
    public void AddUser_Should_Return_When_Normal_User_Has_Exceeded_Credit_Limit()
    {
        
        _userTest.Client = 1;
        _userTest.LastName = "Kowalski"
;
        var userService = new UserService();
        var addResult = userService.AddUser(_userTest.FirstName,_userTest.LastName,_userTest.EmailAddress,_userTest.DateOfBirth, (int)_userTest.Client);
        Assert.False(addResult);
    }

    [Fact]
    public void AddUser_Should_Return_True_When_User_Added_Normally()
    {
        
        _userTest.Client = 2;
        _userTest.LastName = "Malewski";

        var userService = new UserService();
        var addResult = userService.AddUser(_userTest.FirstName,_userTest.LastName,_userTest.EmailAddress,_userTest.DateOfBirth, (int)_userTest.Client);

        _userTest.Client = 3;
        _userTest.LastName = "Smith";

        var addResult2 = userService.AddUser(_userTest.FirstName,_userTest.LastName,_userTest.EmailAddress,_userTest.DateOfBirth, (int)_userTest.Client);
        Assert.True(addResult);
        Assert.True(addResult2);
    }

    [Fact]
    public void AddUser_Should_Throw_When_Using_Non_Existent_Lastname_On_Not_Very_Important_ClientOr_Non_Existent_ClientId()
    {
        _userTest.Client = 10;
        _userTest.LastName = "Malewski";

        var userService = new UserService();

        Assert.Throws<ArgumentException>(() => userService.AddUser(_userTest.FirstName,_userTest.LastName,_userTest.EmailAddress,_userTest.DateOfBirth, (int)_userTest.Client));

        _userTest.Client = 1;
        _userTest.LastName = "Error";

        Assert.Throws<ArgumentException>(() => userService.AddUser(_userTest.FirstName,_userTest.LastName,_userTest.EmailAddress,_userTest.DateOfBirth, (int)_userTest.Client));
    }
    
}