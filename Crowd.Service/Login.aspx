<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Crowd.Service.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="https://apis.google.com/js/platform.js" async defer></script>
    <meta name="google-signin-client_id" content="869978719821-4raar0c7qqjsb0u048olcp6kml8hcq80.apps.googleusercontent.com"/>
    
    <script type="text/javascript">
        function onSignIn(googleUser) {
            var profile = googleUser.getBasicProfile();
            console.log('ID: ' + profile.getId()); // Do not send to your backend! Use an ID token instead.
            console.log('Name: ' + profile.getName());
            console.log('Image URL: ' + profile.getImageUrl());
            console.log('Email: ' + profile.getEmail());

            var id_token = googleUser.getAuthResponse().id_token;
            console.log("ID Token: " + id_token);
        }
    </script>

    <title>Admin Login</title>
</head>
<body>
    <form id="loginForm" runat="server">
        <div>
            <div class="g-signin2" data-onsuccess="onSignIn"></div>
        </div>
    </form>
</body>
</html>
