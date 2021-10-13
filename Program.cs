using System;
using System.IO;
using Console = Log73.Console;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Log73.ExtensionMethod;
using Log73.Extensions;

var campStrings = new[]
{
    @"https://cdn.discordapp.com/attachments/766831166606147626/854546847376736296/Red_Sun_in_the_Sky.mp4
    (我们的) -50 Social Credits have been removed from your account! Bad work citizen, you have not improved your behavior since a few hours ago, you must improve it NOW or you will suffer the consequences! Glory to the CCP.

    Repeated violations of the Behavioral-Identification System of the Social Credit Point system will result in dire consequences, you must improve it now.",
    @"(未成年儿童) -1500 Social Credits have been removed from your account! Bad work citizen, you have not improved your behavior since a few hours ago, you will suffer the consequences! Glory to the CCP!

    Citizen, It seems that your SCT(Social Credit Total) is below the accepted amount, you will now be terminated.
    https://cdn.discordapp.com/attachments/779492573055287357/869597547889586196/mao_zedong-1.mp4",
    @"sent to work camps",
    @"⣿⣿⣿⣿⣿⠟⠋⠄⠄⠄⠄⠄⠄⠄⢁⠈⢻⢿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⠃⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠈⡀⠭⢿⣿⣿⣿⣿
⣿⣿⣿⣿⡟⠄⢀⣾⣿⣿⣿⣷⣶⣿⣷⣶⣶⡆⠄⠄⠄⣿⣿⣿⣿
⣿⣿⣿⣿⡇⢀⣼⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣧⠄⠄⢸⣿⣿⣿⣿
⣿⣿⣿⣿⣇⣼⣿⣿⠿⠶⠙⣿⡟⠡⣴⣿⣽⣿⣧⠄⢸⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣾⣿⣿⣟⣭⣾⣿⣷⣶⣶⣴⣶⣿⣿⢄⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⡟⣩⣿⣿⣿⡏⢻⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣹⡋⠘⠷⣦⣀⣠⡶⠁⠈⠁⠄⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣍⠃⣴⣶⡔⠒⠄⣠⢀⠄⠄⠄⡨⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣦⡘⠿⣷⣿⠿⠟⠃⠄⠄⣠⡇⠈⠻⣿⣿⣿⣿
⣿⣿⣿⣿⡿⠟⠋⢁⣷⣠⠄⠄⠄⠄⣀⣠⣾⡟⠄⠄⠄⠄⠉⠙⠻
⡿⠟⠋⠁⠄⠄⠄⢸⣿⣿⡯⢓⣴⣾⣿⣿⡟⠄⠄⠄⠄⠄⠄⠄⠄
⠄⠄⠄⠄⠄⠄⠄⣿⡟⣷⠄⠹⣿⣿⣿⡿⠁⠄⠄⠄⠄⠄⠄⠄⠄
ATTENTION CITIZEN! 市民请注意!

This is the Central Intelligentsia of the Chinese Communist Party. 您的 Internet 浏览器历史记录和活动引起了我们的注意。 YOUR INTERNET ACTIVITY HAS ATTRACTED OUR ATTENTION. 因此，您的个人资料中的 11115 ( -11115 Social Credits) 个社会积分将打折。 DO NOT DO THIS AGAIN! 不要再这样做! If you do not hesitate, more Social Credits ( -11115 Social Credits )will be subtracted from your profile, resulting in the subtraction of ration supplies. (由人民供应部重新分配 CCP) You'll also be sent into a re-education camp in the Xinjiang Uyghur Autonomous Zone. 如果您毫不犹豫，更多的社会信用将从您的个人资料中打折，从而导致口粮供应减少。 您还将被送到新疆维吾尔自治区的再教育营。

为党争光! Glory to the CCP!",
    @"来自中共的消息（中华人民共和国）在访问资本主义纷争之后，您已经失去了所有的社会信用，公民，干得好。 你被中共开除出党，公开处决。 安全小组将被派往您家进行惩罚。 共产党的荣耀。 - 中华人民共和国 MESSAGE FROM THE CHINESE COMMUNIST PARTY (中华人民共和国) You have lost all your social credit, respect, citizenship and good work after visiting capitalist app Reddit. You are now banished from the CHINESE COMMUNIST PARTY, and shall be publicly executed. A security squad will be sent to your house for a punishment. Glory to the CHINESE COMMUNIST PARTY. -People's Republic of China 注意-习近平看起来不像资本主义偶像小熊维尼 这不是玩笑，你有2天的时间和家人说再见，结果你全家都死了' This is not a joke, you have 2 days to say goodbye to your family, escape will result in your entire family's death"
};
var goodCitizenStrings = new[]
{
    @"MESSAGE FROM THE MINISTRY OF STATE 
(我们的200 Social credits has been added to your account citizen. Your never ending support makes our holy state proud. Glory to the Chinese Communist Party!
https://cdn.discordapp.com/attachments/763740330629398588/865242744162484254/CCP.mp4",
    @"MESSAGE FROM THE MINISTRY OF STATE
+40 social credit, 做得好!
https://cdn.discordapp.com/attachments/779087045033066547/874660794451787776/lol.mp4"
};
Console.Configure.UseNewtonsoftJson();
var client = new DiscordSocketClient();
await client.LoginAsync(TokenType.Bot, await File.ReadAllTextAsync("./token"));
client.Ready += async () =>
{
    Console.Log("REady!");
    var commands = new[]
    {
        new MessageCommandProperties()
        {
            Name = "bad citizen"
        },
        new MessageCommandProperties()
        {
            Name = "cocainer"
        },
        new MessageCommandProperties()
        {
            Name = "good citizen"
        }
    };
    foreach (var cmd in commands)
        await client.CreateGlobalApplicationCommandAsync(cmd);
    Console.Log("COmmands created");
};
var rng = new Random();
client.InteractionCreated += interaction =>
{
    Task.Run(async () =>
    {
        try
        {
            if (interaction is SocketMessageCommand cmd)
            {
                $"Cmd: {cmd.User}: '{cmd.CommandName}'".Dump();
                switch (cmd.CommandName)
                {
                    case "bad citizen":
                        Task.Run(() => interaction.RespondAsync("tr", ephemeral: true));
                        await cmd.Data.Message.Channel.SendMessageAsync(campStrings[rng.Next(campStrings.Length)],
                            messageReference: new(
                                cmd.Data.Message.Id,
                                cmd.Data.Message.Channel.Id));
                        break;
                    case "cocainer":
                        Task.Run(() => interaction.RespondAsync("tr", ephemeral: true));
                        await cmd.Data.Message.Channel.SendMessageAsync(
                            "https://tenor.com/view/cocainer-cat-snow-meme-gif-21371645", messageReference: new(
                                cmd.Data.Message.Id,
                                cmd.Data.Message.Channel.Id));
                        break;
                    case "good citizen":
                        Task.Run(() => interaction.RespondAsync("tr", ephemeral: true));
                        await cmd.Data.Message.Channel.SendMessageAsync(
                            goodCitizenStrings[rng.Next(goodCitizenStrings.Length)], messageReference: new(
                                cmd.Data.Message.Id,
                                cmd.Data.Message.Channel.Id));
                        break;
                }
            }
            else
            {
                await interaction.RespondAsync("Invalid");
            }
        }
        catch (Exception exc)
        {
            Console.Exception(exc);
        }
    });
    return Task.CompletedTask;
};
await client.StartAsync();
await Task.Delay(-1);