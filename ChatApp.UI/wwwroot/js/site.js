var chatHubConnection;

$(document).ready(function () {

    checkSession();

    $('form').submit(function (e) {
        e.preventDefault();
    });

    $(document).on('click', '#btn_Login', function () {
        login();
    });

    $(document).on('keyup', '#txt_NickName', function (e) {

        if (e.which == 13) {

            login();
        }
    });

    $('#txt_Message').focus();

    $('#btn_Send').click(function () {

        sendMessage();
    });

    $('#txt_Message').keyup(function (e) {

        if (e.which == 13) {

            sendMessage();
        }
    });

    $('button[function="create-room"]').click(function () {

        let roomName = $('#txt_RoomName').val();
        createRoom(roomName);
    });

    $('#txt_RoomName').keyup(function (e) {

        if (e.which == 13) {

            let roomName = $('#txt_RoomName').val();
            createRoom(roomName);
        }
    });

    $('button[function="join"]').click(function () {

        var button = $('#lst_ChatRooms li.active').find('button[function="join"]');
        leaveRoom(button);

        joinRoom(this);
    });

    $('button[function="leave"]').click(function () {

        leaveRoom(this);
    });

    $('#mdl_Login').on('shown.bs.modal', function () {

        $('#txt_NickName').focus();
    });

    function sendMessage() {

        var message = $('#txt_Message').val();
        if (!message?.trim()) return;

        let userName = $('#txt_UserName').val();
        let roomName = $('#lst_ChatRooms li.active').find('span.room-name').text();
        let roomId = $('#lst_ChatRooms li.active').find('button[function="join"]').attr('room-id');
        let createdAt = new Date();

        chatHubConnection.invoke('SendMessage', roomId, roomName, userName, message, createdAt).then(function () {

            createdAt = Localization.LocalizateDate(createdAt);
            saveMessage(roomId, message, createdAt);
            $('#txt_Message').val('');
            $('#txt_Message').focus();

        }).catch(function (err) {

            AlertMessage.WithMessage(`İşlem sırasında bir hata meydana geldi\n${err}`);
        });
    }

    function saveMessage(roomId, message, date) {

        let data = {
            roomId: roomId,
            createdAt: date,
            messageText: message,
            userName: $('#txt_UserName').val()
        }

        $.post('/Home/SaveMessage', data).done(function (response) {

            if (response.result == false) {

                AlertMessage.TryAgain();
            }
        }).fail(function () {

            AlertMessage.UnknownError();
        });
    }

    function createMessageBubble(userName, message, date) {

        let bubble = `<div class="d-flex align-items-end flex-column mb-1">
                        <div class="card">
                            <div class="card-header p-1 font-italic"><small>${userName}</small></div>
                            <div class="card-body p-1"><p class="mb-0"><small>${message}</small></p></div>
                            <div class="card-footer p-1"><p class="mb-0"><small>${date}</small></p></div>
                        </div>
                      </div>`;

        return bubble;
    }

    function login() {

        let nickName = $('#txt_NickName').val();
        if (!nickName?.trim()) return;

        $.post(`/Home/SignIn?nickName=${nickName}`).done(function (response) {

            if (response.result == false) {

                AlertMessage.TryAgain();
                return;
            }

            checkSession();

        }).fail(function () {

            AlertMessage.UnknownError();
        });
    }

    function joinRoom(button) {

        let roomId = $(button).attr('room-id');
        let roomName = $(button).closest('li').find('span.room-name').text();

        chatHubConnection.invoke('JoinRoom', roomName).then(function () {

            $('#lst_ChatRooms li').removeClass('active');
            $(button).closest('li').addClass('active');

            $('button[function="join"]').removeAttr('disabled');
            $(button).attr('disabled', true);

            $('#fds_ChatArea').removeAttr('disabled');
            $('#txt_Message').focus();

            getLastMessages(roomId);

        }).catch(function (err) {

            AlertMessage.WithMessage(`İşlem sırasında bir hata meydana geldi\n${err}`);
        });
    }

    function leaveRoom(button) {

        let roomName = $('#lst_ChatRooms li.active').find('span.room-name').text();

        chatHubConnection.invoke('LeaveRoom', roomName).then(function () {

            $(button).closest('li').removeClass('active');

            $('button[function="join"]').removeAttr('disabled');

            $('#fds_ChatArea').attr('disabled', true);

        }).catch(function (err) {

            AlertMessage.WithMessage(`İşlem sırasında bir hata meydana geldi\n${err}`);
        });
    }

    function checkSession() {

        $.get('/Home/CheckSession').done(function (response) {

            if (response.result == false) {

                $('#mdl_Login').modal({
                    backdrop: 'static',
                    keyboard: false
                }, 'show');
            }
            else {

                $('#fds_Page').removeAttr('disabled');
                $('#mdl_Login').modal('hide');
                $('#txt_UserName').val(response?.nickName);
                connectToChatHub();
            }

        }).fail(function () {

            AlertMessage.UnknownError();
        });
    }

    function connectToChatHub() {

        chatHubConnection = new signalR.HubConnectionBuilder().withUrl('/chathub', {
            skipNegotiation: true,
            transport: signalR.HttpTransportType.WebSockets
        }).build();

        startChat();
    }

    function startChat() {

        chatHubConnection.start().then(function () {

            console.log('Hub connection started. Waiting for clients...');
            onReceiveMessage();

        }).catch(function (e) {

            console.log(`Hub connection failed: ${e.statusCode} - ${e.stack}`);

            setTimeout(function () {
                startSignalR();
            }, 3000);
        });
    }

    function onReceiveMessage() {

        chatHubConnection.on('ReceiveMessage', function (roomId, roomName, userName, message, createdAt) {

            let date = Localization.LocalizateDate(createdAt);
            $('#chat_Area').append(createMessageBubble(userName, message, date));
        });
    }

    function getLastMessages(roomId) {

        $('#chat_Area').empty();
        $.get(`/Home/GetLastMessages?roomId=${roomId}`).done(function (response) {

            $.map(response.lastMessages, function (message, i) {

                $('#chat_Area').append(createMessageBubble(message.userName, message.messageText, message.createdAt));
            });

        }).fail(function myfunction() {

            AlertMessage.UnknownError();
        });
    }

    function createRoom(roomName) {

        debugger;
        if (!roomName?.trim()) return;

        $.post('/Home/CreateRoom', { roomName: roomName }).done(function (response) {

            if (response.result == false) {

                AlertMessage.TryAgain();
                return;
            }

            window.location.href = '/Home/Index';

        }).fail(function () {
            AlertMessage.UnknownError();
        });
    }
});

class AlertMessage {

    static UnknownError() {
        alert('Bir hata meydana geldi. Teknik ekibimiz ile iletişime geçin');
    }

    static TryAgain() {
        alert('Bir hata meydana geldi. Lütfen tekrar deneyin');
    }

    static WithMessage(msg) {
        alert(msg);
    }
}

class Localization {

    static LocalizateDate(value) {

        return new Intl.DateTimeFormat('tr-TR', { year: 'numeric', month: 'numeric', day: '2-digit', hour: '2-digit', minute: '2-digit', second: '2-digit' }).format(new Date(value));
    }
}
