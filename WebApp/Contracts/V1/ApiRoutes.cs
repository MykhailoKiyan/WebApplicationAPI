using System.Buffers.Text;

namespace WebApplicationAPI.Contracts.V1 {
  public static class ApiRoutes {
    public const string Root = "api";

    public const string Version = "v1";

    public const string Base = Root + "/" + Version;

    public static class Posts {
      private const string ControllerUrl = Base + "/posts";

      public const string GetAll = ControllerUrl;

      public const string Update = ControllerUrl + "/{postId}";

      public const string Delete = ControllerUrl + "/{postId}";

      public const string Get = ControllerUrl + "/{postId}";

      public const string Create = ControllerUrl;
    }

    public static class Identity {
      private const string ControllerUrl = Base + "/identity";

      public const string Login = ControllerUrl + "/login";

      public const string Register = ControllerUrl + "/register";

      public const string Refresh = ControllerUrl + "/refresh";
    }

    public static class Tags {
      public const string GetAll = Base + "/tags";
    }
  }
}
