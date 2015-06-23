using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
                    DefaultSubscription = true,
                    Title = "Dysfluency",
                    Activities = new List<ParticipantActivity>()
                    {
                        new ParticipantActivity()
                        {
                            ExternalId = "testScenario",
                            Icon = "http://www.survivingamsterdam.com/public/files/e96fc9baf228c0cb8d210a1768995bb1.png",
                            Title = "Getting the Bus",
                            DateSet = DateTime.Now,
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
                            Icon =
                                "http://img3.wikia.nocookie.net/__cb20131231163822/cardfight/images/6/6f/Pizza_slice_combo_clipart.png",
                            Title = "Ordering a Pizza",
                            DateSet = DateTime.Now,
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
                                        Text =
                                            "You are ordering pizza over the phone for both yourself and a friend who has a gluten alergy.\n'Tony's Pizza Parlour, how can I help you?'",
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
                    DefaultSubscription = false,
                    Title = "Dementia",
                    Activities = new List<ParticipantActivity>()
                    {
                        new ParticipantActivity()
                        {
                            ExternalId = "dmentia1",
                            Icon =
                                "http://img3.wikia.nocookie.net/__cb20110528210150/restaurantcity/images/4/46/Spaghetti_Bolognese.png",
                            Title = "Preparing Dinner",
                            DateSet = DateTime.Now,
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
                                        Text =
                                            "You've invited your best friend over for dinner and have decided to make spaghetti bolognese.",
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
                                        Text =
                                            "As you pay for your items, the cashier asks about your bolognese recipe.",
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
                    DefaultSubscription = false,
                    Title = "Helpful Guides",
                    Activities = new List<ParticipantActivity>()
                    {
                        new ParticipantActivity()
                        {
                            ExternalId = "guide1",
                            Icon = "http://www.pursuittraining.co.uk/images/care-icon.gif",
                            Title = "Interaction Tips",
                            DateSet = DateTime.Now,
                            Resource = "https://www.dropbox.com/s/pw1ubz20nwatxtl/guide.zip?raw=1",
                            CrowdPages = new List<ParticipantPage>()
                            {
                                new ParticipantPage()
                                {
                                    MediaLocation = "pic1.jpg",
                                    Text =
                                        "Try to think through how it might feel to struggle to communicate if you were living with dementia and think about what might help and what has helped in the past."
                                },
                                new ParticipantPage()
                                {
                                    MediaLocation = "pic2.jpg",
                                    Text =
                                        "Smile where you can and offer reassuring physical contact where it is appropriate. Make sure people can see your face and that you have engaged their attention."
                                },

                                new ParticipantPage()
                                {
                                    MediaLocation = "pic3.jpg",
                                    Text =
                                        "Relax as much as you can and help the person you are talking with to relax. Be prepared to be treated as someone you are not (for example being mistaken for another relative)."
                                }
                            }
                        }
                    }
                },
                new ParticipantActivityCategory()
                {
                    ExternalId = "Assessments",
                    Icon = "https://cdn1.iconfinder.com/data/icons/MetroStation-PNG/128/MB__help.png",
                    Recommended = false,
                    DefaultSubscription = false,
                    Title = "Progress Assessments",
                    Activities = new List<ParticipantActivity>
                    {
                        new ParticipantActivity
                        {
                            ExternalId = "assess1",
                            Title = "Short Assessment",
                            Description =
                                "Doing this short assessment will help us determine which parts of your speech might need some practice!",
                            DateSet = DateTime.Now,
                            Icon = "http://www.pursuittraining.co.uk/images/care-icon.gif",
                            AssessmentTasks = new List<ParticipantAssessmentTask>
                            {
                                new ParticipantAssessmentTask
                                {
                                    TaskType = ParticipantAssessmentTask.AssessmentTaskType.QuickFire,
                                    Title = "QuickFire Speaking!",
                                    PromptCol = new ParticipantAssessmentTaskPromptCol
                                    {
                                        Name = "Minimal Pairs 1",
                                        PromptType = ParticipantAssessmentTaskPromptCol.PromptTaskType.MinimalPairs,
                                        Prompts = new List<ParticipantAssessmentTaskPrompt>
                                        {
                                            new ParticipantAssessmentTaskPrompt {Value = "Cub"},
                                            new ParticipantAssessmentTaskPrompt {Value = "Coop"},
                                            new ParticipantAssessmentTaskPrompt {Value = "Cup"},
                                            new ParticipantAssessmentTaskPrompt {Value = "Carp"},
                                            new ParticipantAssessmentTaskPrompt {Value = "Keep"},
                                            new ParticipantAssessmentTaskPrompt {Value = "Sheep"},
                                            new ParticipantAssessmentTaskPrompt {Value = "Cape"},
                                            new ParticipantAssessmentTaskPrompt {Value = "Heap"},
                                            new ParticipantAssessmentTaskPrompt {Value = "Cop"},
                                            new ParticipantAssessmentTaskPrompt {Value = "Hub"},
                                        }
                                    }
                                    
                                },
                                new ParticipantAssessmentTask
                                {
                                    TaskType = ParticipantAssessmentTask.AssessmentTaskType.ImageDesc,
                                    Title = "Image Description",
                                    PromptCol = new ParticipantAssessmentTaskPromptCol
                                    {
                                        Name = "Image Description 1",
                                        PromptType = ParticipantAssessmentTaskPromptCol.PromptTaskType.ImagePrompt,
                                        Prompts = new List<ParticipantAssessmentTaskPrompt>
                                        {
                                            new ParticipantAssessmentTaskPrompt
                                            {
                                                Value = "Describe the colours in the image."
                                            },
                                            new ParticipantAssessmentTaskPrompt
                                            {
                                                Value = "Describe the dominant feature of the image."
                                            },
                                            new ParticipantAssessmentTaskPrompt
                                            {
                                                Value = "What does the image make you think of?"
                                            }
                                        }
                                        ,
                                    },
                                    Image =
                                        "http://th00.deviantart.net/fs71/PRE/i/2013/015/d/c/a_hobbit_hole_by_uberpicklemonkey-d5rmn8n.jpg"
                                }
                            }
                        },

                        new ParticipantActivity
                        {
                            ExternalId = "assess2",
                            Title = "Short Assessment",
                            Description =
                                "Doing this short assessment will help us determine which parts of your speech might need some practice!",
                            DateSet = DateTime.Now,
                            Icon = "http://www.pursuittraining.co.uk/images/care-icon.gif",
                            AssessmentTasks = new List<ParticipantAssessmentTask>
                            {
                                new ParticipantAssessmentTask
                                {
                                    TaskType = ParticipantAssessmentTask.AssessmentTaskType.QuickFire,
                                    Title = "QuickFire Speaking!",
                                    PromptCol = new ParticipantAssessmentTaskPromptCol
                                    {
                                        Name = "Minimal Pairs 2",
                                        PromptType = ParticipantAssessmentTaskPromptCol.PromptTaskType.MinimalPairs,
                                        Prompts = new List<ParticipantAssessmentTaskPrompt>
                                        {
                                            new ParticipantAssessmentTaskPrompt {Value = "One"},
                                            new ParticipantAssessmentTaskPrompt {Value = "Fall"},
                                            new ParticipantAssessmentTaskPrompt {Value = "What"},
                                            new ParticipantAssessmentTaskPrompt {Value = "Wash"},
                                            new ParticipantAssessmentTaskPrompt {Value = "Waltz"},
                                            new ParticipantAssessmentTaskPrompt {Value = "Wool"},
                                            new ParticipantAssessmentTaskPrompt {Value = "Watts"},
                                            new ParticipantAssessmentTaskPrompt {Value = "Was"},
                                            new ParticipantAssessmentTaskPrompt {Value = "Wad"},
                                            new ParticipantAssessmentTaskPrompt {Value = "Want"}
                                        }
                                    }
                                    
                                },
                                new ParticipantAssessmentTask
                                {
                                    TaskType = ParticipantAssessmentTask.AssessmentTaskType.ImageDesc,
                                    Title = "Describing Pizza",
                                    PromptCol = new ParticipantAssessmentTaskPromptCol
                                    {
                                        Name = "Image Description 2",
                                        PromptType = ParticipantAssessmentTaskPromptCol.PromptTaskType.ImagePrompt,
                                        Prompts = new List<ParticipantAssessmentTaskPrompt>
                                        {
                                            new ParticipantAssessmentTaskPrompt
                                            {
                                                Value = "Describe your favourite pizza"
                                            },
                                            new ParticipantAssessmentTaskPrompt
                                            {
                                                Value = "Describe the worst pizza you can think of!"
                                            },
                                            new ParticipantAssessmentTaskPrompt
                                            {
                                                Value = "What's your favourite pizza topping and why?"
                                            }
                                        }
                             
                                    },
                                    Image =
                                        "http://guelphpizzawings.ca/wp-content/uploads/2011/12/justeat_14314357.jpg"
                                }
                            }
                        },

                         new ParticipantActivity
                        {
                            ExternalId = "assess3",
                            Title = "Short Assessment",
                            Description =
                                "Doing this short assessment will help us determine which parts of your speech might need some practice!",
                            DateSet = DateTime.Now,
                            Icon = "http://www.pursuittraining.co.uk/images/care-icon.gif",
                            AssessmentTasks = new List<ParticipantAssessmentTask>
                            {
                                new ParticipantAssessmentTask
                                {
                                    TaskType = ParticipantAssessmentTask.AssessmentTaskType.QuickFire,
                                    Title = "QuickFire Speaking!",
                                    PromptCol = new ParticipantAssessmentTaskPromptCol
                                    {
                                        Name = "Minimal Pairs 3",
                                        PromptType = ParticipantAssessmentTaskPromptCol.PromptTaskType.MinimalPairs,
                                        Prompts = new List<ParticipantAssessmentTaskPrompt>
                                        {
                                            new ParticipantAssessmentTaskPrompt {Value = "Bun"},
                                            new ParticipantAssessmentTaskPrompt {Value = "Moon"},
                                            new ParticipantAssessmentTaskPrompt {Value = "Budge"},
                                            new ParticipantAssessmentTaskPrompt {Value = "Botch"},
                                            new ParticipantAssessmentTaskPrompt {Value = "Bond"},
                                            new ParticipantAssessmentTaskPrompt {Value = "Buzz"},
                                            new ParticipantAssessmentTaskPrompt {Value = "Bus"},
                                            new ParticipantAssessmentTaskPrompt {Value = "Bowl"},
                                            new ParticipantAssessmentTaskPrompt {Value = "Butt"},
                                            new ParticipantAssessmentTaskPrompt {Value = "Boss"}
                                        }
                                    }
                                    
                                },
                                new ParticipantAssessmentTask
                                {
                                    TaskType = ParticipantAssessmentTask.AssessmentTaskType.ImageDesc,
                                    Title = "Holiday Memories",
                                    PromptCol = new ParticipantAssessmentTaskPromptCol
                                    {
                                        Name = "Image Description 3",
                                        PromptType = ParticipantAssessmentTaskPromptCol.PromptTaskType.ImagePrompt,
                                        Prompts = new List<ParticipantAssessmentTaskPrompt>
                                        {
                                            new ParticipantAssessmentTaskPrompt
                                            {
                                                Value = "Where do you like to go on holiday, and why?"
                                            },
                                            new ParticipantAssessmentTaskPrompt
                                            {
                                                Value = "What do you think of beach holidays?"
                                            },
                                            new ParticipantAssessmentTaskPrompt
                                            {
                                                Value = "What's your favourite way to travel?"
                                            }
                                        }
                             
                                    },
                                    Image =
                                        "http://luxurytravelspots.com/wp-content/uploads/2013/07/Plane-in-the-sunset.jpg"
                                }
                            }
                        },

                        new ParticipantActivity
                        {
                            ExternalId = "assess4",
                            Title = "Short Assessment",
                            Description =
                                "Doing this short assessment will help us determine which parts of your speech might need some practice!",
                            DateSet = DateTime.Now,
                            Icon = "http://www.pursuittraining.co.uk/images/care-icon.gif",
                            AssessmentTasks = new List<ParticipantAssessmentTask>
                            {
                                new ParticipantAssessmentTask
                                {
                                    TaskType = ParticipantAssessmentTask.AssessmentTaskType.QuickFire,
                                    Title = "QuickFire Speaking!",
                                    PromptCol = new ParticipantAssessmentTaskPromptCol
                                    {
                                        Name = "Minimal Pairs 4",
                                        PromptType = ParticipantAssessmentTaskPromptCol.PromptTaskType.MinimalPairs,
                                        Prompts = new List<ParticipantAssessmentTaskPrompt>
                                        {
                                            new ParticipantAssessmentTaskPrompt {Value = "Mat"},
                                            new ParticipantAssessmentTaskPrompt {Value = "Cat"},
                                            new ParticipantAssessmentTaskPrompt {Value = "Hat"},
                                            new ParticipantAssessmentTaskPrompt {Value = "Fat"},
                                            new ParticipantAssessmentTaskPrompt {Value = "Pat"},
                                            new ParticipantAssessmentTaskPrompt {Value = "Vat"},
                                            new ParticipantAssessmentTaskPrompt {Value = "What"},
                                            new ParticipantAssessmentTaskPrompt {Value = "Gnat"},
                                            new ParticipantAssessmentTaskPrompt {Value = "Tat"},
                                            new ParticipantAssessmentTaskPrompt {Value = "Bat"}
                                        }
                                    }
                                    
                                },
                                new ParticipantAssessmentTask
                                {
                                    TaskType = ParticipantAssessmentTask.AssessmentTaskType.ImageDesc,
                                    Title = "Super Powers",
                                    PromptCol = new ParticipantAssessmentTaskPromptCol
                                    {
                                        Name = "Image Description 4",
                                        PromptType = ParticipantAssessmentTaskPromptCol.PromptTaskType.ImagePrompt,
                                        Prompts = new List<ParticipantAssessmentTaskPrompt>
                                        {
                                            new ParticipantAssessmentTaskPrompt
                                            {
                                                Value = "If you could have a super power, what would it be and why?"
                                            },
                                            new ParticipantAssessmentTaskPrompt
                                            {
                                                Value = "Describe what your costume would be!"
                                            },
                                            new ParticipantAssessmentTaskPrompt
                                            {
                                                Value = "Who's your favourite super hero and why?"
                                            }
                                        }
                                    },
                                    Image =
                                        "http://2.bp.blogspot.com/-CxQhEy0wgbY/TcB-t9G_LwI/AAAAAAAACec/YBAw8oIeWo8/s1600/publicdomain03.jpg"
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

            context.SaveChanges();

            context.Users.Add(new User
            {
                Email = "dan@speeching",
                Avatar = "https://backwardsandstupiddotcom.files.wordpress.com/2012/07/great-success.png",
                Key = "root",
                Name = "Dan Richardson",
                Nickname = "Dan",
                IsAdmin = true,
                SubscribedCategories = context.ParticipantActivityCategories.Where(
                    cat => cat.DefaultSubscription).ToList(),
                Submissions = new List<ParticipantResult>()
            });

            var feedItems = new List<ParticipantFeedItem>
            {
                new ParticipantFeedItem
                {
                    Title = "Welcome to Speeching!",
                    Description =
                        "Speeching is a project designed to help you practice multiple aspects of your speech by completing short assessments and activities. " +
                        "\nYou can choose to upload your results, which will help you by giving you feedback from real people. Use the feedback to highlight areas to practice!",
                    Date = DateTime.Now,
                    Dismissable = true,
                    Importance = 10,
                    Image = "https://www.dropbox.com/s/ks6iuo9ps6umn9e/splashIcon.png?dl=1",
                    Global = true
                },
                
                new ParticipantFeedItem
                {
                    Title = "Dismissing a story",
                    Description =
                        "Swipe a story card (like this one!) to the right to dismiss it from your feed. Please note that some cards are important and can't be dismissed!",
                    Date = DateTime.Now,
                    Dismissable = true,
                    Importance = 9,
                    Image = "https://www.dropbox.com/s/18tvj8x37xxtqsg/arrows.png?dl=1",
                    Global = true
                },

                new ParticipantFeedItem
                {
                    Title = "Featured Article: 'Stuttering is in the genes not the head, say scientists'",
                    Description =
                        "Stuttering is not to do with nervousness or a traumatic childhood as portrayed in the award winning film The King’s Speech but has its root cause in a genetic disorder, new research suggests.",
                    Date = DateTime.Now,
                    Dismissable = true,
                    Importance = 5,
                    Image = "http://i.telegraph.co.uk/multimedia/archive/01830/speech_1830638c.jpg",
                    Interaction = new ParticipantFeedItemInteraction
                    {
                        Type = ParticipantFeedItemInteraction.InteractionType.Url,
                        Value = "http://www.telegraph.co.uk/news/science/science-news/8336493/Stuttering-is-in-the-genes-not-the-head-say-scientists.html",
                        Label = "Read More"
                    },
                    Global = true
                }
            };

            foreach (ParticipantFeedItem item in feedItems)
            {
                context.ParticipantFeedItems.Add(item);
            } 

            context.ActivityHelpers.AddRange(new []
            {
                new ActivityHelper
                {
                    ActivityType = ParticipantAssessmentTask.AssessmentTaskType.ImageDesc,
                    ActivityName = "Topic Prompts",
                    ActivityDescription = "Press the record button and respond to the prompt below the image. Press the button again to finish!"
                },
                new ActivityHelper
                {
                    ActivityType = ParticipantAssessmentTask.AssessmentTaskType.Loudness,
                    ActivityName = "Loudness of Speech",
                    ActivityDescription = "Try to control the volume of your voice, keeping it at a constant volume! " +
                                          "The volume measurement will be red if you're below the volume target, green if you reach it." +
                                          "\nTap 'Set New Target' if you want to raise or lower the target volume.",
                    HelpVideo = "https://openlabdata.blob.core.windows.net/videotuts/volumeTut.mp4"
                },
                new ActivityHelper
                {
                    ActivityType = ParticipantAssessmentTask.AssessmentTaskType.Pacing,
                    ActivityName = "Rate of Speech",
                    ActivityDescription = "If you think you're talking to fast or too slow during conversations, try reading the given passage to the time of the metronome." +
                                          "You can adjust the speed by tapping the up and down arrows!",
                    HelpVideo = "https://openlabdata.blob.core.windows.net/videotuts/pacingTut.mp4"
                },
                new ActivityHelper
                {
                    ActivityType = ParticipantAssessmentTask.AssessmentTaskType.QuickFire,
                    ActivityName = "QuickFire Words",
                    ActivityDescription = "Press the record button and say the shown word as clearly as you can, then press stop."
                },
                new ActivityHelper
                {
                    ActivityType = ParticipantAssessmentTask.AssessmentTaskType.None,
                    ActivityName = "Speeching",
                    ActivityDescription = "Welcome to Speeching! Check for new activities in the Home Feed or practice your speech in the Practice Area.",
                    HelpVideo = "https://openlabdata.blob.core.windows.net/videotuts/welcome.mp4"
                },
            });

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
