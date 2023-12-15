
public class LoginWindow : IUIWindow
{
    public string Account { get; set; }
    public string Password { get; set; }


    public void OnCreate(object uiObj)
    {
        //var loginBtn = GetChild("LoginBtn");
        //loginBtn.OnClick.AddListener(() =>
        //{
        //    AOEvent.Run<LoginEvent>();
        //});
    }

    public void OnOpen()
    {
        throw new System.NotImplementedException();
    }

    public void OnClose()
    {
        throw new System.NotImplementedException();
    }

    public void OnPushDown()
    {
        throw new System.NotImplementedException();
    }

    public void OnPopUp()
    {
        throw new System.NotImplementedException();
    }
}