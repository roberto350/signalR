import React, { useEffect, useState } from 'react'

const Alert = ({ connection }) => {

  const [notificacion, setNotificacion] = useState({});

  const getAlert = async () => {

    if (!connection) return;

    try {

      await connection.start();

      
      connection.on('notificacionesHub', (data) => {
        setNotificacion(data);
        console.log('conectadoX2');
      });

    } catch (error) {
      console.log(error);
    }

  }

  useEffect(() => {
    getAlert();
  }, [connection])

  return (
    <>
      {
        connection && <div> { JSON.stringify(notificacion) } </div>
      }
      {
        connection && <div>{ alert( JSON.stringify(notificacion) ) } </div>
      }
    </>
  )
}

export default Alert