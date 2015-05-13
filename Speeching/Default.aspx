<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpages/Main.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Speeching.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function getProducts() {
            $.getJSON("http://localhost:52215/api/products?category=Hardware",
            function (data) {
                $('#products').empty(); // Clear the table body.

                // Loop through the list of products.
                $.each(data, function (key, val) {
                    // Add a table row for the product.
                    var row = '<td>' + val.Name + '</td><td>' + val.Price + '</td>';
                    $('<tr/>', { text: row })  // Append the name.
                        .appendTo($('#products'));
                });
            });
        }

        $(document).ready(getProducts);
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Button ID="btnHello" runat="server" Text="Go" />
    <asp:Label ID="lblHelloWorld" runat="server"></asp:Label>
    <%--<iframe class="iframe_sound page_margin_top" src="https://w.soundcloud.com/player/?url=https%3A//api.soundcloud.com/tracks/138084493&amp;color=ff5500&amp;auto_play=false&amp;hide_related=false&amp;show_comments=false&amp;show_user=false&amp;show_reposts=false"></iframe>--%>
    <div class="wrapper col2">
        <div id="gallery" style="width: 100%; padding-top: 0px; padding-bottom: 0px">
            <div id="firstRow">
                <%--
<?

//if ($detect->isMobile() || $detect->isTablet()) 
//{
//  echo "<img style='height:60%;max-height:90%;width:80%;max-width:90%;opacity: 0.7;' src='images/whisper-vertical.png'/>";
//}
//else
//{
 echo "<img style='width:100%;opacity: 0.7;' src='images/whisper.png'/>";
//}


?>--%>
            </div>
        </div>
        <div id="secondRow">
            <a name="about"></a>
            <div style="width: 100%; text-align: left; padding-top: 50px">
                <h2>What is Speeching?</h2>
                <p>
                    Speeching is a experimental platform that enables everyone to support everyday speech improvements for people with Parkinson&apos;s. People with Parkinson&apos;s often experience speech problems, for example, speaking quietly, slurring and having reduced expression in their voice. All of these issues affect a person&apos;s ability to be understood by others which can be distressing and embarrassing, particularly when speaking with strangers.
Speech and language therapists are trained to focus upon these difficulties, their time is limited. Speeching is designed to explore the benefits of wider participation (from people like you!) to support everyday speech improvement
                </p>
            </div>

            <div id="middle" style="margin-top: 50px; display: block;">

                <div id="why">
                    <h2>Why should I join?</h2>
                    <p>
                        You will be directly participating in the design and testing of a platform we hope to roll out to the world in 2015. In future this could help us gather vital information on the types of speech difficulties that people with Parkinson&apos;s have on a daily basis, and how other people (like you!) perceive those problems. 
With your feedback and participation we can make the platform more fun to use, and useful to people with parkinson&apos;s
                    </p>
                </div>

                <div id="skills">
                    <h2>Do I need any special skills?</h2>
                    <p>
                        Nope. No special skills are necessary. Your contribution is valuable as it helps us to understand the degree to which a person 
with Parkinson&apos;s might be understood by a stranger encountered in their everyday lives. Speech therapists are also welcome to join
too.
We are interested in widening participation in the everyday support networks of people with Parkinson&apos;s.
                    </p>
                </div>

                <div style="display: block; clear: both;"></div>

            </div>

            <div style="margin-top: 50px;">
                <h2>What will I be asked to do?</h2>
                <p>
                    On the site we ask people to listen to short sections of speech and and answer simple questions. The tasks on the site are broken into 
very small parts and you could be asked to perform very short transcriptions of text, rate how easy it was to listen to a section of speech, 
or even make scores around how 'breathy' a section of speech sounds. We have lots of things for you to do whether you have 5 minutes of time to spare 
or 50 minutes. Currently, we are in the experiment phase and would be really happy if you could try, over time, to do 100% of the tasks on the site. This will help
us to refine the system for our public launch.

                </p>
            </div>

        </div>
        <%--<?
if (!$detect->isMobile() && !$detect->isTablet())
{
  echo "<div id='thirdRow' style='margin-top:100px;background-color:white;padding-top:50px;'>";
  echo "<img style='width:100%' src='images/tasks.png'/>";
  echo "</div>";
}
?>
<? 
/*
if (!$detect->isMobile() && !$detect->isTablet())
{
?>
<div style="padding-top:50px;float:right;width:50%;text-align:right;padding-right:50px;background">
<img style="width:100%" src="images/mp.png"/>
</div>
<?
}
*/
?>--%>
    </div>
    <div class="wrapper col4">
        <div id="container">
            <div id="content">
                <!--
      <h1>About This Free CSS Template</h1>
      <p>This is a W3C standards compliant free website template from <a href="http://www.os-templates.com/">OS Templates</a>.</p>
      <p>This template is distributed using a <a href="http://www.os-templates.com/template-terms">Website Template Licence</a>, which allows you to use and modify the template for both personal and commercial use when you keep the provided credit links in the footer.</p>
      <p>For more CSS templates visit <a href="http://www.os-templates.com/">Free Website Templates</a>.</p>
      <p>Lacusenim inte trices lorem anterdum nam sente vivamus quis fauctor mauris. Wisinon vivamus wisis adipis laorem lobortis curabiturpiscingilla dui platea ipsum lacingilla.</p>
      <p>Semalique tor sempus vestibulum libero nibh pretium eget eu elit montes. Sedsemporttis sit intesque felit quis elis et cursuspenatibulum tincidunt non curabitae.</p>
      <div class="homecontent">
        <ul>
          <li>
            <p class="imgholder"><img src="images/demo/286x100.gif" alt="" /></p>
            <h2>Indonectetus facilis leo nibh</h2>
            <p>Nullamlacus dui ipsum conseque loborttis non euisque morbi penas dapibulum orna. Urnaultrices quis curabitur phasellentesque.</p>
            <p>congue magnis vestibulum quismodo nulla et feugiat. Adipisciniapellentum leo ut consequam ris felit elit id nibh sociis malesuada.</p>
            <p class="readmore"><a href="#">Read More &raquo;</a></p>
          </li>

          <li>
            <p class="imgholder"><img src="images/demo/286x100.gif" alt="" /></p>
            <h2>Indonectetus facilis leo nibh</h2>
            <p>Nullamlacus dui ipsum conseque loborttis non euisque morbi penas dapibulum orna. Urnaultrices quis curabitur phasellentesque.</p>
            <p>congue magnis vestibulum quismodo nulla et feugiat. Adipisciniapellentum leo ut consequam ris felit elit id nibh sociis malesuada.</p>
            <p class="readmore"><a href="#">Read More &raquo;</a></p>
          </li>
          
          <li class="last">
            <p class="imgholder"><img src="images/demo/286x100.gif" alt="" /></p>
            <h2>Indonectetus facilis leo nibh</h2>
            <p>Nullamlacus dui ipsum conseque loborttis non euisque morbi penas dapibulum orna.</p>
            <p>Urnaultrices quis curabitur phasellentesque congue magnis vestibulum quismodo nulla et feugiat. Adipisciniapellentum leo ut consequam ris felit elit id nibh sociis malesuada.</p>
            <p class="readmore"><a href="#">Read More &raquo;</a></p>
          </li>
        </ul>
        <div class="clear"></div>
      </div>
      <p>Odiointesque at quat nam nec quis ut feugiat consequet orci liberos. Tempertincidunt sed maecenas eros elerit nullam vest rhoncus diam consequat amet. Diamdisse ligula tincidunt a orci proin auctor lacilis lacilis met vitae.</p>
      -->
            </div>

            <!--<div id="column">
      <div id="featured">
        <ul>
          <li>
            <h2>Indonectetus facilis leonib</h2>
            <p class="imgholder"><img src="images/demo/240x90.gif" alt="" /></p>
            <p>Nullamlacus dui ipsum conseque loborttis non euisque morbi penas dapibulum orna. Urnaultrices quis curabitur phasellentesque congue magnis vestibulum quismodo nulla et feugiat. Adipisciniapellentum leo ut consequam ris felit elit id nibh sociis malesuada.</p>
            <p class="more"><a href="#">Read More &raquo;</a></p>
          </li>
        </ul>
      </div>
      <div class="holder">
        <div class="imgholder"><img src="images/demo/290x100.gif" alt="" /></div>
        <p>Nullamlacus dui ipsum conseque loborttis non euisque morbi penas dapibulum orna.</p>
        <p class="readmore"><a href="#">Read More &raquo;</a></p>
      </div>
    </div>-->
            <div class="clear"></div>
        </div>
    </div>

    <div class="wrapper col5">
        <div id="footer">

            <div class="clear"></div>
        </div>
        <!-- End Company Details -->

        <div class="clear"></div>
    </div>
</asp:Content>
