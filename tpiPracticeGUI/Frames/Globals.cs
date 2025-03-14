using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Threading;
using tpiPracticeClasses;
using System.Net.Http;
using System.Net;
using System.Text.Json;
using System.Diagnostics;
using System.Windows;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using System.Security.Cryptography;
using System.Net.WebSockets;

namespace tpiPracticeGUI.Frames
{
    class Globals
    {

        public static event EventHandler OnPhaseChanged = delegate { };

        public static event EventHandler OnGroupchatRemove = delegate { };
        public static event EventHandler OnGroupchatEdit = delegate { };
        public static event EventHandler OnGroupchatAdd = delegate { };

        public static event EventHandler OnUserDisconnect = delegate { };

        public static event EventHandler OnGroupchatJoin = delegate { };

        public static string apiURL = "http://localhost:5165/api";

        public enum Phase
        {
            login,
            signup,
            groupchatListing,
            chatting
        };

        private static Phase _phase = Phase.login;

        private static User? _currentLoggedUser = null;
        public static User? currentLoggedUser
        {
            get => _currentLoggedUser;
            set
            {
                _currentLoggedUser = value;

                if (value == null)
                {
                    OnUserDisconnect(null, EventArgs.Empty);
                }
            }
        }

        private static Groupchat? _currentlyJoinedGroupchat = null;
        public static Groupchat? currentJoinedGroupchat
        {
            get => _currentlyJoinedGroupchat;
            set
            {
                _currentlyJoinedGroupchat = value;
                OnGroupchatJoin(null, EventArgs.Empty);
            }
        }

        public static ObservableCollection<Message> currentMessageList = new ObservableCollection<Message>();

        private static ObservableCollection<Groupchat> groupchatList = new ObservableCollection<Groupchat>();

        public static void setGroupchatList(ObservableCollection<Groupchat> _groupchatList)
        {
            groupchatList = _groupchatList;
        }

        public static void addGroupchat(Groupchat groupchat)
        {
            App.Current?.Dispatcher.Invoke((Action)delegate {
                groupchatList.Add(groupchat);
            });
        }

        public static void removeGroupchat(Groupchat groupchat)
        {

            for (int i = 0;  i < groupchatList.Count; i++)
            {
                if (groupchatList[i] == groupchat)
                {
                    App.Current?.Dispatcher.Invoke((Action)delegate {
                        groupchatList.RemoveAt(i);
                    });
                    return;
                }
            }
        }

        public static bool isSameAsCachedGroupchatList(List<Groupchat> list)
        {
            if (list.Count != Globals.groupchatList.Count)
            {
                return false;
            }

            for (int i = 0; i < list.Count; i++)
            {
                Groupchat actualGroupchat = list[i];
                Groupchat cachedGroupchat = Globals.groupchatList[i];

                bool hasSameId = actualGroupchat.GroupchatId == cachedGroupchat.GroupchatId;
                bool hasSameName = actualGroupchat.Name == cachedGroupchat.Name;
                bool hasSameUserId = actualGroupchat.UserId == cachedGroupchat.UserId;

                if (!hasSameId || !hasSameName || !hasSameUserId)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool isSameAsCachedMessagetList(List<Message> list)
        {
            if (list.Count != Globals.currentMessageList.Count)
            {
                return false;
            }

            for (int i = 0; i < list.Count; i++)
            {
                Message actualMessage = list[i];
                Message cachedMessage = Globals.currentMessageList[i];

                bool hasSameId = actualMessage.messageId == cachedMessage.messageId;
                bool hasSameContent = actualMessage.content == cachedMessage.content;
                bool hasSameUserId = actualMessage.userId == cachedMessage.userId;

                if (!hasSameId || !hasSameContent || !hasSameUserId)
                {
                    return false;
                }
            }

            return true;
        }

        public static void editGroupchatById(int id, Groupchat groupchat)
        {
            groupchatList.FirstOrDefault(x => x.GroupchatId == groupchat.GroupchatId)!.Name = groupchat.Name;
            groupchatList.FirstOrDefault(x => x.GroupchatId == groupchat.GroupchatId)!.UserId = groupchat.UserId;
            groupchatList.FirstOrDefault(x => x.GroupchatId == groupchat.GroupchatId)!.GroupchatId = groupchat.GroupchatId;
        }

        public static ObservableCollection<Groupchat> getGroupChatList => groupchatList;

        public static Phase phase
        {
            get { return _phase; }
            set {
                _phase = value;
                OnPhaseChanged(null, EventArgs.Empty);
            }
        }
    }
}
