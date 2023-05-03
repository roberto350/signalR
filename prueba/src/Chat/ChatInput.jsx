import React, {useState} from 'react';

const ChatInput = (props) => {
    const [user, setUser] = useState('');
    const [message, setMessage] = useState('');
    const [group, setGroup] = useState('');

    const onSubmit = (e) => {
        e.preventDefault();

        const isUserProvided = user && user !== '';
        const isMessageProvided = message && message !== '';
        const isGroupProvided = group && group !== '';

        if( isUserProvided && isMessageProvided && isGroupProvided){
            props.sendMessage(user,message,group);
        } else {
            alert('Por favor inserte un usuario y contraseña')
        }
    }

    const onUserUpdate = (e) => {
        setUser(e.target.value)
    }

    const onMessageUpdate = (e) => {
        setMessage(e.target.value)
    }

    const onGroupUpdate = (e) => {
        setGroup(e.target.value)
    }

    return(
        <form
            onSubmit ={onSubmit}>

            <label htmlFor='Group'>Group:</label>
            <br/>
            <input
                type = "text"
                id = "group"
                name = "group"
                value = {group} 
                onChange = {onGroupUpdate} />
            <br/>

            <label htmlFor='user'>User:</label>
            <br/>
            <input
                id = "user"
                name = "user"
                value = {user}
                onChange = {onUserUpdate} />
            <br/>


            <label htmlFor='message'>Message:</label>
            <br/>
            <input
                type = "text"
                id = "message"
                name = "message"
                value = {message} 
                onChange = {onMessageUpdate} />
            <br/>
            <br/>
                <button>Submit</button>
        </form>
    )
};

export default ChatInput;