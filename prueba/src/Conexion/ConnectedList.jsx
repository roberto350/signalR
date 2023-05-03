import { HubConnectionBuilder } from '@microsoft/signalr';
import React, { useEffect, useState } from 'react'
import Alert from '../notificaciones/Alert';

const ConnectedList = () => {

  const [connection, setConnection] = useState(null);
  const [totalConnected, setTotalConnected] = useState(0);
   const origen ="Monitor";
   
  const updateConnected = async () => {

    if (!connection) return;

    try {

      connection
        .start()
        .then( () => {
          connection?.invoke("JoinGrp",origen);
      }).catch( (error) => console.log(error));


      connection.on('ConexionMessage', (data) => {
        setTotalConnected(data);
      });

      connection.on('DesconexionMessage', (data) => {
      console.log(`Conectados: ${data}`);
      setTotalConnected(data);
      setConnection(null);
      });

    } catch (error) {
      console.log(error);
    }
  }

  console.log(connection);
  useEffect(() => {
    const url = 'https:localhost:7017/hubs/chat/'
    //const url = 'https://message.qapkt1i.cf/hubs/chat'
    // const url = 'https://desktop-1et5qim:7017/hubs/chat'

    const newConnection = new HubConnectionBuilder()
      .withUrl(url)
      .withAutomaticReconnect()
      .build();

    setConnection(newConnection);
    return () => {
      newConnection.stop().then( ()=> console.log('Desconectado D:') )
    }
    
  }, []);

  useEffect(() => {
    updateConnected();
  }, [connection]);

  // console.log(connection);
  return (
    <div>
      <p> weyes conectados {totalConnected}</p>
      {
        connection && <Alert conn={connection}/>
      }
      
      <Alert connection={connection}/>
    </div>
    /*<Stack alignItems="center" p={3}>
      <Box sx={{ minWidth: 300, p: 2, borderRadius: 4, boxShadow: 3 }}>
        {
          connection?._connectionState == "Connecting"
            ? <Center> <CircularProgress /></Center>
            : <Stack direction="row" justifyContent="space-between" alignItems="center">
              <Typography>{`Conectados (${totalConnected})`}</Typography>
              <Box sx={{ width: 13, height: 13, bgcolor: 'success.light', borderRadius: 10, boxShadow: 2 }} />
            </Stack>
        }

      </Box>
    </Stack>*/
  )
}

export default ConnectedList;
