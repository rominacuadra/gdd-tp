﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FrbaHotel.ClasesTabla
{
    class DHotel
    {
        EntidadBase entidadBase = new EntidadBase();

        public List<Hotel> GetAll()
        {
            DataTable us = entidadBase.TraerDatos("hotel.SP_HOTELES");
            List<Hotel> hoteles = new List<Hotel>();

            foreach (DataRow r in us.Rows)
            {
                FrbaHotel.ClasesTabla.Hotel hotel = new FrbaHotel.ClasesTabla.Hotel();
                hotel.codigo = r.Field<decimal>("codigo");
                hotel.nombre = r.Field<string>("nombre");
                hotel.mail = r.Field<string>("mail");
                hotel.telefono = r.Field<decimal>("telefono");
                hotel.cant_estrellas = r.Field<decimal>("cant_estrellas");
                hotel.nom_calle = r.Field<string>("nom_calle");
                hotel.num_calle = r.Field<decimal>("num_calle");
                hotel.pais = r.Field<string>("pais");
                hotel.ciudad = r.Field<string>("ciudad");
                hotel.fecha_creacion = r.Field<DateTime>("fecha_creacion");
                hotel.administrador = r.Field<decimal>("administrador");
                hotel.recarga_estrella = r.Field<decimal>("recarga_estrella");
                hoteles.Add(hotel);
            }
            return hoteles;
        }

        public List<Hotel> GetAllActivos()
        {
            DataTable us = entidadBase.TraerDatos("hotel.SP_GETHOTELESACTIVOS");
            List<Hotel> hoteles = new List<Hotel>();

            foreach (DataRow r in us.Rows)
            {
                FrbaHotel.ClasesTabla.Hotel hotel = new FrbaHotel.ClasesTabla.Hotel();
                hotel.codigo = r.Field<decimal>("codigo");
                hotel.nombre = r.Field<string>("nombre");
                hotel.mail = r.Field<string>("mail");
                hotel.telefono = r.Field<Nullable<decimal>>("telefono");
                hotel.cant_estrellas = r.Field<decimal>("cant_estrellas");
                hotel.nom_calle = r.Field<string>("nom_calle");
                hotel.num_calle = r.Field<decimal>("num_calle");
                hotel.pais = r.Field<string>("pais");
                hotel.ciudad = r.Field<string>("ciudad");
                hotel.fecha_creacion = r.Field<DateTime>("fecha_creacion");
                hotel.administrador = r.Field<Nullable<decimal>>("administrador");
                hotel.recarga_estrella = r.Field<decimal>("recarga_estrella");
                hoteles.Add(hotel);
            }
            return hoteles;
        }

        public Hotel GetByDireccion(string direccion)
        {
               List<Hotel> hoteles = GetAll();
               Hotel hotel = (from u in hoteles where u.direccionCompleta == direccion select u).SingleOrDefault();
               return hotel;
        } 

        private string ArmarValores(Hotel hotel)
        {
            StringBuilder Valores = new StringBuilder();
            if (hotel.codigo != 0)
                Valores.Append(hotel.codigo + ",");
            Valores.Append("'" + hotel.nombre + "',");
            Valores.Append("'" + hotel.mail + "',");
            Valores.Append("" + hotel.telefono + ",");
            Valores.Append("" + hotel.cant_estrellas + ",");
            Valores.Append("'" + hotel.nom_calle + "',");
            Valores.Append("" + hotel.num_calle + ",");
            Valores.Append("'" + hotel.pais + "',");
            if (hotel.fecha_creacion != null)
                Valores.Append("'" + hotel.fecha_creacion.ToShortDateString() + "',");
            else
                Valores.Append("'" + DateTime.Now.ToShortDateString() + "',");
            Valores.Append("" + hotel.administrador + ",");
            Valores.Append("" + hotel.recarga_estrella + "");
            return Valores.ToString();
        }

        public List<HotelDTO> GetBySQLGRID(string consulta)
        {
            DataTable us = entidadBase.TraerDatos(consulta);
            List<HotelDTO> hoteles = new List<HotelDTO>();

            foreach (DataRow r in us.Rows)
            {
                FrbaHotel.ClasesTabla.HotelDTO hotel = new FrbaHotel.ClasesTabla.HotelDTO();
                hotel.codigo = r.Field<decimal>("codigo");
                hotel.nombre = r.Field <string>("nombre");
                hotel.mail = r.Field <string>("mail");
                hotel.telefono = r.Field<Nullable<decimal>>("telefono");
                hotel.cant_estrellas = r.Field<decimal>("cant_estrellas");
                hotel.nom_calle = r.Field<string>("nom_calle");
                hotel.num_calle = r.Field<decimal>("num_calle");
                hotel.pais = r.Field<string>("pais");
                hotel.ciudad = r.Field<string>("ciudad");
                hoteles.Add(hotel);
            }
            return hoteles;
        }

        public List<Hotel> GetBySQL(string consulta)
        {
            DataTable us = entidadBase.TraerDatos(consulta);
            List<Hotel> hoteles = new List<Hotel>();

            foreach (DataRow r in us.Rows)
            {
                FrbaHotel.ClasesTabla.Hotel hotel = new FrbaHotel.ClasesTabla.Hotel();
                hotel.codigo = r.Field<decimal>("codigo");
                hotel.nombre = r.Field <string>("nombre");
                hotel.mail = r.Field <string>("mail");
                hotel.telefono = r.Field<Nullable<decimal>>("telefono");
                hotel.cant_estrellas = r.Field<decimal>("cant_estrellas");
                hotel.nom_calle = r.Field<string>("nom_calle");
                hotel.num_calle = r.Field<decimal>("num_calle");
                hotel.pais = r.Field<string>("pais");
                hotel.ciudad = r.Field<string>("ciudad");
                hotel.fecha_creacion = r.Field<DateTime>("fecha_creacion");
                hotel.administrador = r.Field <Nullable<decimal>>("administrador");
                hotel.recarga_estrella = r.Field<decimal>("recarga_estrella");
                hoteles.Add(hotel);
            }
            return hoteles;
        }

        public Hotel GetByCod(decimal cod)
        {
            Hotel hotel = GetBySQL("select * from hotel.Hotel where codigo =" + cod).SingleOrDefault();
            return hotel;
        }

        public bool hotelOcupado(decimal cod, DateTime desde, DateTime hasta)
        {
            bool hayGente = false;
            DataTable us = entidadBase.TraerDatos("select count(*) cantidad from hotel.Reserva where cod_hotel = " + cod + "and (fecha_desde > " + hasta.ToShortDateString() + " or fecha_hasta < " + desde.ToShortDateString() + " )"  );
            int cantidadReserva = 0;

            foreach (DataRow r in us.Rows)
            {
                cantidadReserva = r.Field<int>("cantidad");
            }
            if (cantidadReserva != 0) hayGente = true;

            return hayGente;
        }

        public bool hotelOcupadoRegimen(decimal codHotel, decimal codReg, DateTime fechaHoy)
        {
            bool hayGente = false;
            DataTable us = entidadBase.TraerDatos("select count(*) cantidad from hotel.Reserva where cod_hotel = " + codHotel + "and fecha_hasta < " + fechaHoy.ToShortDateString() + " and  cod_regimen = " + codReg);
            int cantidadReserva = 0;

            foreach (DataRow r in us.Rows)
            {
                cantidadReserva = r.Field<int>("cantidad");
            }
            if (cantidadReserva != 0) hayGente = true;

            return hayGente;
        }

        public decimal Save(Hotel hotel)
        {
            int ex = entidadBase.EjecutarSQL("hotel.SP_INSHOTEL " + ArmarValores(hotel));
            DataTable resultID = entidadBase.TraerDatos("SELECT max(codigo) from hotel.Hotel ");
            DataRow row = resultID.Rows[0];
            return Convert.ToInt32(row[0]);
        }

        public void SaveCancelacion(CancelacionHotel cancHotel)
        {
            int ex = entidadBase.EjecutarSQL("insert hotel.Cancelacion_Hotel (cod_hotel,fecha_desde,fecha_hasta,motivo) values (" + cancHotel.cod_hotel.ToString() + ","+cancHotel.fecha_desde.ToString() + "," + cancHotel.fecha_hasta.ToString() + "," + cancHotel.motivo + ")" );
        }

        public void Update(Hotel hotel)
        {
            int ex = entidadBase.EjecutarSQL("hotel.SP_UPDHOTEL " + ArmarValores(hotel));
        }

        public void Delete(int codigo)
        {
            int ex = entidadBase.EjecutarSQL("hotel.SP_DELUSER " + codigo);
        }
    }
}