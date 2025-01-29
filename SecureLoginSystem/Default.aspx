<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication1.Default" %>

<!DOCTYPE html>
<html>
<head>
    <title>Home</title>
 
     <script>
         let timeLeft = <%= Session.Timeout * 60 %>; // Get session timeout in seconds from the server

         const startCountdown = () => {
             const countdownElement = document.getElementById("countdown");
             const timerInterval = setInterval(() => {
                 // Decrease time left
                 timeLeft--;

                 // Update the countdown display
                 const minutes = Math.floor(timeLeft / 60);
                 const seconds = timeLeft % 60;
                 countdownElement.innerText = `Session will expire in ${minutes}:${seconds < 10 ? '0' : ''}${seconds}`;

                 // If time runs out, redirect to the logout page
                 if (timeLeft <= 0) {
                     clearInterval(timerInterval);
                     window.location.href = "Login.aspx";
                 }
             }, 1000);
         };

         // Start the countdown
         window.onload = startCountdown;
    </script>

</head>
<body>
    <h2>Welcome, <%= Context.User.Identity.Name %></h2>
    <div id="countdown" style="font-size: 20px; color: red;"></div>
    <form id="form1" runat="server">
        <asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="btnLogout_Click" />
    </form>
</body>
</html>