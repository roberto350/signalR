import React, { useState, useEffect, useRef} from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';

import ChatWindow  from './ChatWindow'
import ChatInput from "./ChatInput";


const Chat = () => {
    const [connection, setConnection] = useState(null);
    const [chat, setChat] = useState([]);
    const latestChat = useRef(null);

    latestChat.current = chat;

    useEffect(() =>{

        const newConnection = new HubConnectionBuilder()
        .withUrl('https://localhost:7017/hubs/chat/')
        .withAutomaticReconnect()
        .build();

        setConnection(newConnection);
    }, []);


    useEffect( () => {
        if (connection){
            connection.start()
            .then( result => {
                console.log('Connected!');

                connection.on('ReceiveMessage', message => {
                    const updateChat = [...latestChat.current];
                    updateChat.push(message);

                    setChat(updateChat);
                });
            })
            .catch( e => console.log('Connection failed: ', e))
        }
    },[connection]);


    

    const sendMessage = async (user, message ,group) => {
        
        const chatMessage = {
            user : user,
            message : message,
            group : group
        };

        
        
        if(connection?._connectionStarted){
            try{
                await connection.send('SendMessage', chatMessage);
                console.log(connection);
            }catch(e){
                console.log(e);
            }
        }
        else{
            alert('No conection to server yet.');
        }
    }

    return(
        <div>
            <ChatInput sendMessage = {sendMessage} />
            <hr/>
            <ChatWindow chat = {chat}/>
        </div>
    );
};

export default Chat;