using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tinder_lvl10.Data;

namespace API.Controllers
{
    [Authorize]
    public class MessagesController : BaseApiController
    {
        
        private readonly IUserRepository userRepository;
        private readonly IMessageRepository messageRepository;
        private readonly IMapper mapper;

        public MessagesController(DataContext context, IUserRepository userRepository, IMessageRepository messageRepository, IMapper mapper) : base(context)
        {
            
            this.userRepository = userRepository;
            this.messageRepository = messageRepository;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDTO>> CreateMessage(CreateMessageDTO createMessageDTO) {

            var username = User.GetUsername();

            if (username.ToLower() == createMessageDTO.RecipientUsername.ToLower()) {

                return BadRequest("You cannot send messages to yourself");
            }

            var sender = await userRepository.GetUserByUsernameAsync(username);
            var recipient = await userRepository.GetUserByUsernameAsync(createMessageDTO.RecipientUsername);

            if (recipient == null) return NotFound();

            var message = new Message
            {

                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDTO.Content

            };
            

            messageRepository.AddMessage(message);
            var url1 = "";
            var url2 = "";

            var senderURL = sender.Photos.FirstOrDefault(x => x.IsMain, null);
            if (senderURL != null) {
                url1 = senderURL.Url;
            }
            

            var recipientURL = recipient.Photos.FirstOrDefault(x => x.IsMain, null);
            if (recipientURL != null)
            {
                url2 =recipientURL.Url;
            }




            var messagetoreturn = new MessageDTO
            {
                Id = message.Id,
                SenderId = message.SenderId,
                SenderUsername = sender.UserName,
                SenderPhotoURL = url1,

            RecipientId = message.RecipientId,
                RecipientUsername = message.RecipientUsername,
                RecipientPhotoURL =url2,


                Content = message.Content,
                DateRead = message.DateRead,
                MessageSent = message.MessageSent,
            };

            if (await messageRepository.SaveAllAsync()) return Ok(messagetoreturn);

            return BadRequest("Failed to send the message");

        
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessageForUser([FromQuery] MessageParams messageParams) {

            messageParams.Username = User.GetUsername();

            var messages = await messageRepository.GetMessagesForUser(messageParams);

            Response.AddPaginationHeader(messages.CurrentPage, messages.PageSize, messages.Totalcount, messages.TotalPages);

            return messages;
        
        
        }

        [HttpGet("thread/{username}")]

        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessageThread(string username) {



            var currentUsername = User.GetUsername();
            return Ok(await messageRepository.GetMessageThread(currentUsername, username));
        
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> DeleteMessage(int id) {

            var username = User.GetUsername();

            var message = await messageRepository.GetMessage(id);

            if (message.Sender.UserName != username && message.Recipient.UserName != username) return Unauthorized();

            if (message.Sender.UserName == username) message.SenderDeleted = true;
            if (message.Recipient.UserName == username) message.RecpientDeleted = true;

            if (message.SenderDeleted && message.RecpientDeleted) messageRepository.DeleteMessage(message);

            if (await messageRepository.SaveAllAsync()) return Ok();

            return BadRequest("Problem deleteing the message");
        
        
        }





    }
}
