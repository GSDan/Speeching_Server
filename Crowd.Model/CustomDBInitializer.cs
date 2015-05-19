using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Crowd.Model.Data;

namespace Crowd.Model
{
    public class CustomDBInitializer : DropCreateDatabaseIfModelChanges<CrowdContext>
    {
        protected override void Seed(CrowdContext context)
        {
            //if (System.Diagnostics.Debugger.IsAttached == false)
            //    System.Diagnostics.Debugger.Launch();
            Console.WriteLine("*****Seeding******");
            var categories = new List<ParticipantActivityCategory>();
            categories.AddRange(new List<ParticipantActivityCategory>()
            {
                new ParticipantActivityCategory()
                {
                    ExternalId = "anId1",
                    Icon = "https://cdn0.iconfinder.com/data/icons/cosmo-medicine/40/test-tube_1-128.png",
                    Recommended = false,
                    Title = "Dysfluency",
                    Activities = new List<ParticipantActivity>()
                    {
                      new ParticipantActivity()
                      {
                          ExternalId = "testScenario",
                          Icon = "http://www.survivingamsterdam.com/public/files/e96fc9baf228c0cb8d210a1768995bb1.png",
                          Title = "Getting the Bus",
                          Resource = "https://www.dropbox.com/s/0h2f8pyrh6xte3s/bus.zip?raw=1",
                          ParticipantTasks = new List<ParticipantTask>()
                          {
                              new ParticipantTask()
                              {
                                  ExternalId = "sc1ev1",
                                  Title = "",
                                  Description = "",
                                  ParticipantTaskContent = new ParticipantTaskContent()
                                  {
                                      Text = "Hello! Where would you like to go today?",
                                      Type = "Audio",
                                      Audio = "hello.mp3",
                                      Visual = "driver.jpg"
                                  },
                                  ParticipantTaskResponse = new ParticipantTaskResponse()
                                  {
                                      Type = "Prompted",
                                      Prompt = "Hello, please may I have a return ticket to the train station?"
                                  }
                              },
                              new ParticipantTask()
                              {
                                  ExternalId = "sc1ev2",
                                  Title = "",
                                  Description = "",
                                  ParticipantTaskContent = new ParticipantTaskContent()
                                  {
                                      Text = "Hello! Where would you like to go today?",
                                      Type = "Audio",
                                      Audio = "thanks.mp3",
                                      Visual = "driver.jpg"
                                  },
                                  ParticipantTaskResponse = new ParticipantTaskResponse()
                                  {
                                      Type = "Prompted",
                                      Prompt = "Thank you. Have a good day."
                                  }
                              },
                              new ParticipantTask()
                              {
                                  ExternalId = "sc1ev13",
                                  Title = "",
                                  Description = "",
                                  ParticipantTaskContent = new ParticipantTaskContent()
                                  {
                                      Text = "You sit next to an old woman, who asks what your plans are for the day.",
                                      Type = "text",
                                      Audio = "",
                                      Visual = "oldwoman.jpg"
                                  },
                                  ParticipantTaskResponse = new ParticipantTaskResponse()
                                  {
                                      Type = "Freeform",
                                      Prompt = "Greet her and explain that you're catching a train to the seaside."
                                  }
                              }
                          }
                      },
                      new ParticipantActivity()
                      {
                          ExternalId = "testScenario2",
                          Icon = "http://img3.wikia.nocookie.net/__cb20131231163822/cardfight/images/6/6f/Pizza_slice_combo_clipart.png",
                          Title = "Ordering a Pizza",
                          Resource = "https://www.dropbox.com/s/8gt7pqh6zq6p18h/pizza.zip?raw=1",
                          ParticipantTasks = new List<ParticipantTask>()
                          {
                              new ParticipantTask()
                              {
                                  ExternalId = "sc2ev1",
                                  Title = "",
                                  Description = "",
                                  ParticipantTaskContent = new ParticipantTaskContent()
                                  {
                                      Text = "You are ordering pizza over the phone for both yourself and a friend who has a gluten alergy.\n'Tony's Pizza Parlour, how can I help you?'",
                                      Type = "Audio",
                                      Audio = "hello.mp3",
                                      Visual = "phone.jpg"
                                  },
                                  ParticipantTaskResponse = new ParticipantTaskResponse()
                                  {
                                      Type = "Prompted",
                                      Prompt = "Hello, can I order a pizza please?"
                                  }
                              },
                              new ParticipantTask()
                              {
                                  ExternalId = "sc2ev2",
                                  Title = "",
                                  Description = "",
                                  ParticipantTaskContent = new ParticipantTaskContent()
                                  {
                                      Text = "Of course! What kind would you like?",
                                      Type = "Audio",
                                      Audio = "order1.mp3",
                                      Visual = "pizza.jpg"
                                  },
                                  ParticipantTaskResponse = new ParticipantTaskResponse()
                                  {
                                      Type = "Freeform",
                                      Prompt = "Describe your favourite pizza."
                                  }
                              },
                              new ParticipantTask()
                              {
                                  ExternalId = "sc2ev3",
                                  Title = "",
                                  Description = "",
                                  ParticipantTaskContent = new ParticipantTaskContent()
                                  {
                                      Text = "That sounds delicious! Would you like anything else?",
                                      Type = "Audio",
                                      Audio = "order2.mp3",
                                      Visual = "pizza.jpg"
                                  },
                                  ParticipantTaskResponse = new ParticipantTaskResponse()
                                  {
                                      Type = "Freeform",
                                      Prompt = "Describe another kind of pizza, but make sure it's gluten free!"
                                  }
                              },
                              new ParticipantTask()
                              {
                                  ExternalId = "sc2ev4",
                                  Title = "",
                                  Description = "",
                                  ParticipantTaskContent = new ParticipantTaskContent()
                                  {
                                      Text = "No problem at all, we can do that. See you soon!",
                                      Type = "Audio",
                                      Audio = "end.mp3",
                                      Visual = "making.jpg"
                                  },
                                  ParticipantTaskResponse = new ParticipantTaskResponse()
                                  {
                                      Type = "Prompted",
                                      Prompt = "Thank you, see you later."
                                  }
                              }
                          }
                      }
                    }
                },
                new ParticipantActivityCategory()
                {
                    ExternalId = "anId2",
                    Icon = "https://cdn0.iconfinder.com/data/icons/cosmo-medicine/40/test-tube_1-128.png",
                    Recommended = false,
                    Title = "Dementia",
                    Activities = new List<ParticipantActivity>()
                    {
                      new ParticipantActivity()
                      {
                          ExternalId = "dmentia1",
                          Icon = "http://img3.wikia.nocookie.net/__cb20110528210150/restaurantcity/images/4/46/Spaghetti_Bolognese.png",
                          Title = "Preparing Dinner",
                          Resource = "https://www.dropbox.com/s/3isleqzen5gt0hf/dinner.zip?raw=1",
                          ParticipantTasks = new List<ParticipantTask>()
                          {
                              new ParticipantTask()
                              {
                                  ExternalId = "sc3ev1",
                                  Title = "",
                                  Description = "",
                                  ParticipantTaskContent = new ParticipantTaskContent()
                                  {
                                      Text = "You've invited your best friend over for dinner and have decided to make spaghetti bolognese.",
                                      Type = "Audio",
                                      Audio = "spag1.mp3",
                                      Visual = "spagBol.jpg"
                                  },
                                  ParticipantTaskResponse = new ParticipantTaskResponse()
                                  {
                                      Type = "none",
                                      Prompt = ""
                                  }
                              },
                              new ParticipantTask()
                              {
                                  ExternalId = "sc3ev2",
                                  Title = "",
                                  Description = "",
                                  ParticipantTaskContent = new ParticipantTaskContent()
                                  {
                                      Text = "You go to the supermarket to buy some of the ingredients.",
                                      Type = "Audio",
                                      Audio = "spag2.mp3",
                                      Visual = "supermarket.jpg"
                                  },
                                  ParticipantTaskResponse = new ParticipantTaskResponse()
                                  {
                                      Type = "Choice",
                                      Prompt = "Choose the spaghetti from the shelf.",
                                      Related = "spaghetti.jpg|bakedBeans.png"
                                  }
                              },
                              new ParticipantTask()
                              {
                                  ExternalId = "sc3ev3",
                                  Title = "",
                                  Description = "",
                                  ParticipantTaskContent = new ParticipantTaskContent()
                                  {
                                      Text = "You also need something to make the sauce from...",
                                      Type = "Audio",
                                      Audio = "spag3.mp3",
                                      Visual = "supermarket.jpg"
                                  },
                                  ParticipantTaskResponse = new ParticipantTaskResponse()
                                  {
                                      Type = "Choice",
                                      Prompt = "Which of these could you make a pasta sauce from?",
                                      Related = "dogFood.jpg|tomatoes.jpg"
                                  }
                              },
                              new ParticipantTask()
                              {
                                  ExternalId = "sc3ev4",
                                  Title = "",
                                  Description = "",
                                  ParticipantTaskContent = new ParticipantTaskContent()
                                  {
                                      Text = "As you pay for your items, the cashier asks about your bolognese recipe.",
                                      Type = "Audio",
                                      Audio = "spag4.mp3",
                                      Visual = "cashier.jpg"
                                  },
                                  ParticipantTaskResponse = new ParticipantTaskResponse()
                                  {
                                      Type = "Freeform",
                                      Prompt = "Describe the process of cooking spaghetti bolognese to the cashier."
                                  }
                              },
                              new ParticipantTask()
                              {
                                  ExternalId = "sc3ev5",
                                  Title = "",
                                  Description = "",
                                  ParticipantTaskContent = new ParticipantTaskContent()
                                  {
                                      Text = "Oh, that sounds delicious! Are you having anyone over?",
                                      Type = "Audio",
                                      Audio = "spag5.mp3",
                                      Visual = "cashier.jpg"
                                  },
                                  ParticipantTaskResponse = new ParticipantTaskResponse()
                                  {
                                      Type = "Freeform",
                                      Prompt = "Describe your best friend to the cashier."
                                  }
                              }
                          }
                      }
                    }
                },
                new ParticipantActivityCategory()
                {
                    ExternalId = "anId3",
                    Icon = "https://cdn1.iconfinder.com/data/icons/MetroStation-PNG/128/MB__help.png",
                    Recommended = false,
                    Title = "Helpful Guides",
                    Activities = new List<ParticipantActivity>()
                    {
                      new ParticipantActivity()
                      {
                          ExternalId = "guide1",
                          Icon = "http://www.pursuittraining.co.uk/images/care-icon.gif",
                          Title = "Interaction Tips",
                          Resource = "https://www.dropbox.com/s/pw1ubz20nwatxtl/guide.zip?raw=1",
                          CrowdPages = new List<ParticipantPage>()
                          {
                              new ParticipantPage()
                              {
                                  MediaLocation = "pic1.jpg",
                                  Text = "Try to think through how it might feel to struggle to communicate if you were living with dementia and think about what might help and what has helped in the past."
                              },
                              new ParticipantPage()
                              {
                                  MediaLocation = "pic2.jpg",
                                  Text = "Smile where you can and offer reassuring physical contact where it is appropriate. Make sure people can see your face and that you have engaged their attention."
                              },

                              new ParticipantPage()
                              {
                                  MediaLocation = "pic3.jpg",
                                  Text = "Relax as much as you can and help the person you are talking with to relax. Be prepared to be treated as someone you are not (for example being mistaken for another relative)."
                              }
                          }
                      }
                    }
                }
            });
            Console.WriteLine("*****Adding******");
            foreach (var cat in categories)
            {
                context.ParticipantActivityCategories.Add(cat);    
            }
            
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                Console.WriteLine("*****" + msg + "******");
            }
            Console.WriteLine("*****done******");
            base.Seed(context);
        }
    }
}
