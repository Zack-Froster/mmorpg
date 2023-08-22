using Common;
using Common.Utils;
using Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    class GuildService : Singleton<GuildService>
    {
        public GuildService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildCreateRequest>(this.OnGuildCreate);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildListRequest>(this.OnGuildList);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildJoinRequest>(this.OnGuildJoinRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildJoinResponse>(this.OnGuildJoinResponse);

            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildLeaveRequest>(this.OnGuildLeave);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildAdminRequest>(this.OnGuildAdmin);
        }
        public void Init()
        {
            GuildManager.Instance.Init();
        }

        private void OnGuildCreate(NetConnection<NetSession> sender, GuildCreateRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnGuildCreate: :GuildName:{0} character:[{1}]{2}", request.GuildName, character.Id, character.Name);
            sender.Session.Response.guildCreate = new GuildCreateResponse();
            if (character.Guild != null)
            {
                sender.Session.Response.guildCreate.Result = Result.Failed;
                sender.Session.Response.guildCreate.Errormsg = "已经有公告";
                sender.SendResponse();
                return;
            }
            if (GuildManager.Instance.CheckNameExisted(request.GuildName))
            {
                sender.Session.Response.guildCreate.Result = Result.Failed;
                sender.Session.Response.guildCreate.Errormsg = "公会名称已存在";
                sender.SendResponse();
                return;
            }

            GuildManager.Instance.CreateGuild(request.GuildName, request.GuildNotice, character);
            sender.Session.Response.guildCreate.guildInfo = character.Guild.GuildInfo(character);
            sender.Session.Response.guildCreate.Result = Result.Success;
            sender.SendResponse();
        }

        private void OnGuildList(NetConnection<NetSession> sender, GuildListRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnGuildList: character:[{0}]{1}", character.Id, character.Name);

            sender.Session.Response.guildList = new GuildListResponse();
            sender.Session.Response.guildList.Guilds.AddRange(GuildManager.Instance.GetGuildsInfo());
            sender.Session.Response.guildList.Result = Result.Success;
            sender.SendResponse();
        }


        /// <summary>
        /// 收到加公会请求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="request"></param>
        private void OnGuildJoinRequest(NetConnection<NetSession> sender, GuildJoinRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnCuildJoinRequest: :Guild:{0} characterId:[{1}]{2}", request.Apply.GuildId, request.Apply.GuildId, request.Apply.characterId, request.Apply.Name);
            var guild = GuildManager.Instance.GetGuild(request.Apply.GuildId);
            if (guild == null)
            {
                sender.Session.Response.guildJoinRes = new GuildJoinResponse();
                sender.Session.Response.guildJoinRes.Result = Result.Failed;
                sender.Session.Response.guildJoinRes.Errormsg = "公会不存在";
                sender.SendResponse();
                return;
            }
            request.Apply.characterId = character.Data.ID;
            request.Apply.Name = character.Data.Name;
            request.Apply.Class = character.Data.Class;
            request.Apply.Level = character.Data.Level;

            if (guild.JoinApply(request.Apply))
            {
                var leader = SessionManager.Instance.GetSession(guild.Data.LeaderID);
                if (leader != null)
                {
                    //给会长发送申请加入请求
                    leader.Session.Response.guildJoinReq = request;
                    leader.SendResponse();
                }
            }
            else
            {
                sender.Session.Response.guildJoinRes = new GuildJoinResponse();
                sender.Session.Response.guildJoinRes.Result = Result.Failed;
                sender.Session.Response.guildJoinRes.Errormsg = "请勿重复申请";
            }
        }

        /// <summary>
        /// 收到加公会响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="response"></param>
        private void OnGuildJoinResponse(NetConnection<NetSession> sender, GuildJoinResponse response)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnGuildJoinResponse: : GuildId:{0} characterId:[{1}]{2}", response.Apply.GuildId, response.Apply.characterId, response.Apply.Name);

            var guild = GuildManager.Instance.GetGuild(response.Apply.GuildId);
            if (response.Result == Result.Success)
            {
                //接受了公会请求
                guild.JoinApprove(response.Apply);
            }

            var requester = SessionManager.Instance.GetSession(response.Apply.characterId);
            if (requester != null)
            {
                requester.Session.Character.Guild = guild;
                //通知请求者加入成功
                requester.Session.Response.guildJoinRes = response;
                requester.Session.Response.guildJoinRes.Result = Result.Success;
                requester.Session.Response.guildJoinRes.Errormsg = "加入公会成功";
                requester.SendResponse();
            }
        }

        private void OnGuildLeave(NetConnection<NetSession> sender, GuildLeaveRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnGuildLeave: :character:{0}", character.Id);
            sender.Session.Response.guildLeave = new GuildLeaveResponse();

            character.Guild.Leave(character);
            sender.Session.Response.guildLeave.Result = Result.Success;

            DBService.Instance.Save();

            sender.SendResponse();
        }

        private void OnGuildAdmin(NetConnection<NetSession> sender, GuildAdminRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnGuildLeave: :character:{0}", character.Id);
            sender.Session.Response.guildAdmin = new GuildAdminResponse();
            if (character.Guild == null)
            {
                sender.Session.Response.guildAdmin.Result = Result.Failed;
                sender.Session.Response.guildAdmin.Errormsg = "你没公会不要乱来";
                sender.SendResponse();
                return;
            }

            var sourceItem = character.Guild.Data.Members.FirstOrDefault(v => v.CharacterId == character.Id);
            var targetItem = character.Guild.Data.Members.FirstOrDefault(v => v.CharacterId == message.Target);
            if (message.Command == GuildAdminCommand.Kickout)
            {
                if (sourceItem != null)
                {
                    if (sourceItem.Title < (int)GuildTitle.VicePresident)
                    {
                        sender.Session.Response.guildAdmin.Result = Result.Failed;
                        sender.Session.Response.guildAdmin.Errormsg = "权限不足，至少是副会长哦";
                        sender.SendResponse();
                        return;
                    }
                    if (targetItem != null)
                    {
                        if (sourceItem.Title <= targetItem.Title)
                        {
                            sender.Session.Response.guildAdmin.Result = Result.Failed;
                            sender.Session.Response.guildAdmin.Errormsg = "你无权踢出这名玩家";
                            sender.SendResponse();
                            return;
                        }
                        if(sourceItem.Id == sourceItem.Guild.LeaderID && targetItem.Id == targetItem.Guild.LeaderID && sourceItem.Guild.LeaderID == targetItem.Guild.LeaderID)
                        {
                            sender.Session.Response.guildAdmin.Result = Result.Failed;
                            sender.Session.Response.guildAdmin.Errormsg = "会长不能踢自己哦";
                            sender.SendResponse();
                            return;
                        }
                    }
                }
            }
            if (message.Command != GuildAdminCommand.Kickout && sourceItem.Title < (int)GuildTitle.President)
            {
                sender.Session.Response.guildAdmin.Result = Result.Failed;
                sender.Session.Response.guildAdmin.Errormsg = "你不是会长";
                sender.SendResponse();
                return;
            }


            character.Guild.ExecuteAdmin(message.Command, message.Target, character.Id);

            var target = SessionManager.Instance.GetSession(message.Target);
            if (target != null)
            {
                target.Session.Response.guildAdmin = new GuildAdminResponse();
                target.Session.Response.guildAdmin.Result = Result.Success;
                target.Session.Response.guildAdmin.Command = message;
                target.SendResponse();
            }

            sender.Session.Response.guildAdmin.Result = Result.Success;
            sender.Session.Response.guildAdmin.Command = message;
            sender.SendResponse();
            
        }

        public void GuildMemberStatusChange(Character character)
        {
           if (character.Guild == null) return;
           foreach(var mem in character.Guild.Data.Members)
            {
                var session = SessionManager.Instance.GetSession(mem.CharacterId);
                if (session!= null)
                {
                    session.Session.Character.Guild.timestamp = TimeUtil.timestamp;
                }
            }
        }

    }
}
