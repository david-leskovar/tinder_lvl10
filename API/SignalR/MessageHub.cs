using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class MessageHub : Hub
    {

        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public MessageHub(IMessageRepository messageRepository, IMapper mapper,IUserRepository userRepository) {

            _mapper = mapper;
            _messageRepository = messageRepository;
           _userRepository = userRepository;
        

        }

        public override async Task OnConnectedAsync() {

            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext.Request.Query["user"].ToString();

            var groupName = GetGroupName(Context.User.GetUsername(), otherUser);

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var messages = await _messageRepository.GetMessageThread(Context.User.GetUsername(), otherUser);

            await Clients. Group(groupName).SendAsync("ReceiveMessageThread", messages);
        
        }

        public override async Task OnDisconnectedAsync(Exception ex) {


            await base.OnDisconnectedAsync(ex);

        
        }

        public async Task SendMessage(CreateMessageDTO createMessageDTO) {


            var username = Context.User.GetUsername();

            if (username.ToLower() == createMessageDTO.RecipientUsername.ToLower())
            {

                throw new HubException("You cannot send messages to yourself");
            }

            var sender = await _userRepository.GetUserByUsernameAsync(username);
            var recipient = await _userRepository.GetUserByUsernameAsync(createMessageDTO.RecipientUsername);

            if (recipient == null) throw new HubException("Not found user");

            var message = new Message
            {

                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDTO.Content

            };


            _messageRepository.AddMessage(message);
            var url1 = "";
            var url2 = "";

            var senderURL = sender.Photos.FirstOrDefault(x => x.IsMain, null);
            if (senderURL != null)
            {
                url1 = senderURL.Url;
            }


            var recipientURL = recipient.Photos.FirstOrDefault(x => x.IsMain, null);
            if (recipientURL != null)
            {
                url2 = recipientURL.Url;
            }




            var messagetoreturn = new MessageDTO
            {
                Id = message.Id,
                SenderId = message.SenderId,
                SenderUsername = sender.UserName,
                SenderPhotoURL = url1,

                RecipientId = message.RecipientId,
                RecipientUsername = message.RecipientUsername,
                RecipientPhotoURL = url2,


                Content = message.Content,
                DateRead = message.DateRead,
                MessageSent = message.MessageSent,
            };

            if (await _messageRepository.SaveAllAsync()) {

                var group = GetGroupName(sender.UserName, recipient.UserName);
                await Clients.Group(group).SendAsync("NewMessage", messagetoreturn);

            
            }

            





        }


        private string GetGroupName(string caller, string other) {

            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        
        }

    }
}
