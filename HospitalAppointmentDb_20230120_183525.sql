--
-- PostgreSQL database dump
--

-- Dumped from database version 15.1
-- Dumped by pg_dump version 15rc2

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: public; Type: SCHEMA; Schema: -; Owner: postgres
--

-- *not* creating schema, since initdb creates it


ALTER SCHEMA public OWNER TO postgres;

--
-- Name: SCHEMA public; Type: COMMENT; Schema: -; Owner: postgres
--

COMMENT ON SCHEMA public IS '';


--
-- Name: appointment_reserved(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.appointment_reserved() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
IF NEW.patient_id IS NOT NULL THEN
OLD.is_available = false;
    INSERT INTO unavailable_appointments (appointment_date, doctor_id, end_time, patient_id, start_time) SELECT (appointment_date, doctor_id, appointment_end_time, NEW.patient_id, appointment_time) from available_appointments where is_available = false AND NEW.patient_id is not null;
END IF;
RETURN NEW;
END;
$$;


ALTER FUNCTION public.appointment_reserved() OWNER TO postgres;

--
-- Name: check_registration(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.check_registration() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
  --INSERT INTO patients ("first_name","last_name") VALUES ("NEW.first_name","NEW.last_name");
  --IF (length(NEW.social_id)/4>10 AND length(NEW.social_id)/4<10) THEN
    --RAISE EXCEPTION 'your social ID is invalid';
  --END IF;
  --IF EXISTS (SELECT 1 from users WHERE social_id = "NEW.social_id") THEN RAISE EXCEPTION 'your social ID is invalid';
  --END IF;
  --IF (isnumeric(NEW.first_name) == 'true' OR isnumeric(NEW.last_name) == 'true')THEN
    --RAISE EXCEPTION 'first and last name informations are invalid';
  --END IF;--
  --RETURN NEW;
END;
$$;


ALTER FUNCTION public.check_registration() OWNER TO postgres;

--
-- Name: create_appointments(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.create_appointments() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
DECLARE
    doctor doctors%ROWTYPE;
    start_time TIMESTAMP;
    hospital INTEGER;
    date DATE;
BEGIN

FOR doctor IN SELECT * FROM doctors WHERE hospital_id = NEW.hospital_id LOOP
FOR start_time IN SELECT generate_series(
(SELECT CAST(current_date + INTERVAL '9 hours' AS TIMESTAMP)),
(SELECT CAST(current_date + INTERVAL '16 hours' AS TIMESTAMP)),
INTERVAL '1 hour'
) LOOP
FOR i IN 1..7 LOOP
INSERT INTO available_appointments (doctor_id, appointment_time, appointment_end_time, hospital_id, appointment_date)
VALUES (doctor.doctor_id, start_time + INTERVAL '1 hour' * i, start_time + INTERVAL '1 hour' * (i + 1), doctor.hospital_id, current_date + INTERVAL '5 days' * i);
END LOOP;
END LOOP;
END LOOP;
RETURN NEW;
END;
$$;


ALTER FUNCTION public.create_appointments() OWNER TO postgres;

--
-- Name: delete_hospital(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.delete_hospital() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
UPDATE doctors SET hospital_id = NULL WHERE hospital_id = OLD.hospital_id;
RETURN OLD; 
END;
$$;


ALTER FUNCTION public.delete_hospital() OWNER TO postgres;

--
-- Name: move_available_appointments(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.move_available_appointments() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
IF NEW.is_available is true THEN
INSERT INTO available_appointments ( doctor_id, appointment_time, appointment_end_time, appointment_date,hospital_id, is_available) SELECT doctor_id, start_time, end_time, appointment_date,hospital_id, is_available FROM  unavailable_appointments WHERE is_available = NEW.is_available;
DELETE FROM unavailable_appointments where is_available = NEW.is_available;
end if;
return new;
END;
$$;


ALTER FUNCTION public.move_available_appointments() OWNER TO postgres;

--
-- Name: move_unavailable_appointments(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.move_unavailable_appointments() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
  IF NEW.patient_id IS NOT NULL THEN
    INSERT INTO unavailable_appointments (doctor_id, patient_id, start_time, end_time, appointment_date,appointment_id,hospital_id)
    SELECT doctor_id, patient_id, appointment_time, appointment_end_time, appointment_date, appointment_id,hospital_id FROM available_appointments WHERE appointment_id = NEW.appointment_id;
    UPDATE unavailable_appointments SET is_available = false WHERE patient_id = new.patient_id;
    UPDATE unavailable_appointments SET book_date = NOW() WHERE patient_id = new.patient_id;
    DELETE FROM available_appointments where patient_id = new.patient_id;
  END IF;
  RETURN NEW;
END;
$$;


ALTER FUNCTION public.move_unavailable_appointments() OWNER TO postgres;

--
-- Name: prevent_duplicate_appointments(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.prevent_duplicate_appointments() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
IF (SELECT COUNT(*) FROM available_appointments WHERE patient_id = NEW.patient_id AND appointment_date = NEW.appointment_date AND appointment_time = NEW.appointment_time) > 0 THEN
RAISE EXCEPTION 'This appointment is already reserved';
END IF;
return new;
END;
$$;


ALTER FUNCTION public.prevent_duplicate_appointments() OWNER TO postgres;

--
-- Name: prevent_passed_appointments(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.prevent_passed_appointments() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
IF (NEW.appointment_date < NOW()) THEN
RAISE EXCEPTION 'This appointment is already passed';
END IF;
RETURN NEW;
END;
$$;


ALTER FUNCTION public.prevent_passed_appointments() OWNER TO postgres;

--
-- Name: user_registered(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.user_registered() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    INSERT INTO patients (first_name, last_name, user_id)
SELECT first_name, last_name, user_id
FROM users
WHERE user_role = 'P';
    INSERT INTO doctors (first_name, last_name, user_id)
SELECT first_name, last_name, user_id
FROM users
WHERE user_role = 'D';
    RETURN NEW;
END;
$$;


ALTER FUNCTION public.user_registered() OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: available_appointments; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.available_appointments (
    appointment_id integer NOT NULL,
    appointment_date date,
    appointment_time time without time zone,
    is_available boolean DEFAULT true,
    doctor_id integer,
    patient_id integer,
    hospital_id integer,
    appointment_end_time time without time zone
);


ALTER TABLE public.available_appointments OWNER TO postgres;

--
-- Name: appointments_appointmentID_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."appointments_appointmentID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."appointments_appointmentID_seq" OWNER TO postgres;

--
-- Name: appointments_appointmentID_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."appointments_appointmentID_seq" OWNED BY public.available_appointments.appointment_id;


--
-- Name: counties; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.counties (
    county_id integer NOT NULL,
    name character varying(500) NOT NULL,
    province_id integer NOT NULL
);


ALTER TABLE public.counties OWNER TO postgres;

--
-- Name: counties_county_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.counties_county_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.counties_county_id_seq OWNER TO postgres;

--
-- Name: counties_county_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.counties_county_id_seq OWNED BY public.counties.county_id;


--
-- Name: doctors; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.doctors (
    doctor_id integer NOT NULL,
    first_name character varying(255) NOT NULL,
    last_name character varying(255) NOT NULL,
    policlinic_id integer,
    phone character varying(255),
    address character varying(255),
    user_id integer,
    hospital_id integer
);


ALTER TABLE public.doctors OWNER TO postgres;

--
-- Name: doctors_doctor_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.doctors_doctor_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.doctors_doctor_id_seq OWNER TO postgres;

--
-- Name: doctors_doctor_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.doctors_doctor_id_seq OWNED BY public.doctors.doctor_id;


--
-- Name: hospitals; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.hospitals (
    hospital_id integer NOT NULL,
    hospital_name character varying(255) NOT NULL,
    county_id integer NOT NULL,
    phone character varying(255) NOT NULL
);


ALTER TABLE public.hospitals OWNER TO postgres;

--
-- Name: hospitals_hospital_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.hospitals_hospital_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.hospitals_hospital_id_seq OWNER TO postgres;

--
-- Name: hospitals_hospital_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.hospitals_hospital_id_seq OWNED BY public.hospitals.hospital_id;


--
-- Name: insurancecompanies; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.insurancecompanies (
    insurancecompany_id integer NOT NULL,
    insurancecompany_name character varying(50) NOT NULL,
    contact text NOT NULL
);


ALTER TABLE public.insurancecompanies OWNER TO postgres;

--
-- Name: insurancecompanies_insurancecompany_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.insurancecompanies_insurancecompany_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.insurancecompanies_insurancecompany_id_seq OWNER TO postgres;

--
-- Name: insurancecompanies_insurancecompany_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.insurancecompanies_insurancecompany_id_seq OWNED BY public.insurancecompanies.insurancecompany_id;


--
-- Name: medicalhistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.medicalhistory (
    medicalhistory_id integer NOT NULL,
    patient_id integer NOT NULL,
    hospital_id integer NOT NULL,
    doctor_id integer NOT NULL,
    diagnosis character varying(200) NOT NULL,
    prescripton_id integer NOT NULL
);


ALTER TABLE public.medicalhistory OWNER TO postgres;

--
-- Name: medicalhistory_medicalhistory_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.medicalhistory_medicalhistory_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.medicalhistory_medicalhistory_id_seq OWNER TO postgres;

--
-- Name: medicalhistory_medicalhistory_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.medicalhistory_medicalhistory_id_seq OWNED BY public.medicalhistory.medicalhistory_id;


--
-- Name: patients; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.patients (
    patient_id integer NOT NULL,
    first_name character varying(255),
    last_name character varying(255),
    phone character varying(255),
    address character varying(255),
    user_id integer
);


ALTER TABLE public.patients OWNER TO postgres;

--
-- Name: patients_patient_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.patients_patient_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.patients_patient_id_seq OWNER TO postgres;

--
-- Name: patients_patient_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.patients_patient_id_seq OWNED BY public.patients.patient_id;


--
-- Name: policlinics; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.policlinics (
    policlinic_id integer NOT NULL,
    policlinic_name character varying(200),
    hospital_id integer
);


ALTER TABLE public.policlinics OWNER TO postgres;

--
-- Name: policlinics_policlinic_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.policlinics_policlinic_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.policlinics_policlinic_id_seq OWNER TO postgres;

--
-- Name: policlinics_policlinic_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.policlinics_policlinic_id_seq OWNED BY public.policlinics.policlinic_id;


--
-- Name: prescriptions; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.prescriptions (
    prescripton_id integer NOT NULL,
    patient_id integer NOT NULL,
    doctor_id integer,
    medicines character varying(200) NOT NULL,
    dosage character varying(100) NOT NULL,
    frequency character varying(200) NOT NULL,
    duration integer NOT NULL
);


ALTER TABLE public.prescriptions OWNER TO postgres;

--
-- Name: prescriptions_prescripton_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.prescriptions_prescripton_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.prescriptions_prescripton_id_seq OWNER TO postgres;

--
-- Name: prescriptions_prescripton_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.prescriptions_prescripton_id_seq OWNED BY public.prescriptions.prescripton_id;


--
-- Name: provinces; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.provinces (
    province_id integer NOT NULL,
    name character varying(255) NOT NULL
);


ALTER TABLE public.provinces OWNER TO postgres;

--
-- Name: provinces_province_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.provinces_province_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.provinces_province_id_seq OWNER TO postgres;

--
-- Name: provinces_province_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.provinces_province_id_seq OWNED BY public.provinces.province_id;


--
-- Name: unavailable_appointments; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.unavailable_appointments (
    id integer NOT NULL,
    doctor_id integer NOT NULL,
    patient_id integer,
    start_time time without time zone NOT NULL,
    end_time time without time zone NOT NULL,
    appointment_date date,
    book_date date,
    hospital_id integer,
    appointment_id integer,
    is_available boolean DEFAULT false
);


ALTER TABLE public.unavailable_appointments OWNER TO postgres;

--
-- Name: unavailable_appointments_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.unavailable_appointments_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.unavailable_appointments_id_seq OWNER TO postgres;

--
-- Name: unavailable_appointments_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.unavailable_appointments_id_seq OWNED BY public.unavailable_appointments.id;


--
-- Name: users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.users (
    user_id integer NOT NULL,
    first_name character varying(50) NOT NULL,
    password character varying(255) NOT NULL,
    user_role character(1) NOT NULL,
    last_name character varying(20) NOT NULL,
    social_id character varying(10) NOT NULL,
    adress text,
    gender character(1),
    birth_date date,
    insurancecompany_id integer,
    county_id integer
);


ALTER TABLE public.users OWNER TO postgres;

--
-- Name: user_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.user_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.user_id_seq OWNER TO postgres;

--
-- Name: user_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.user_id_seq OWNED BY public.users.user_id;


--
-- Name: available_appointments appointment_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.available_appointments ALTER COLUMN appointment_id SET DEFAULT nextval('public."appointments_appointmentID_seq"'::regclass);


--
-- Name: counties county_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.counties ALTER COLUMN county_id SET DEFAULT nextval('public.counties_county_id_seq'::regclass);


--
-- Name: doctors doctor_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.doctors ALTER COLUMN doctor_id SET DEFAULT nextval('public.doctors_doctor_id_seq'::regclass);


--
-- Name: hospitals hospital_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.hospitals ALTER COLUMN hospital_id SET DEFAULT nextval('public.hospitals_hospital_id_seq'::regclass);


--
-- Name: insurancecompanies insurancecompany_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.insurancecompanies ALTER COLUMN insurancecompany_id SET DEFAULT nextval('public.insurancecompanies_insurancecompany_id_seq'::regclass);


--
-- Name: medicalhistory medicalhistory_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.medicalhistory ALTER COLUMN medicalhistory_id SET DEFAULT nextval('public.medicalhistory_medicalhistory_id_seq'::regclass);


--
-- Name: patients patient_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.patients ALTER COLUMN patient_id SET DEFAULT nextval('public.patients_patient_id_seq'::regclass);


--
-- Name: policlinics policlinic_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.policlinics ALTER COLUMN policlinic_id SET DEFAULT nextval('public.policlinics_policlinic_id_seq'::regclass);


--
-- Name: prescriptions prescripton_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.prescriptions ALTER COLUMN prescripton_id SET DEFAULT nextval('public.prescriptions_prescripton_id_seq'::regclass);


--
-- Name: provinces province_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.provinces ALTER COLUMN province_id SET DEFAULT nextval('public.provinces_province_id_seq'::regclass);


--
-- Name: unavailable_appointments id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.unavailable_appointments ALTER COLUMN id SET DEFAULT nextval('public.unavailable_appointments_id_seq'::regclass);


--
-- Name: users user_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users ALTER COLUMN user_id SET DEFAULT nextval('public.user_id_seq'::regclass);


--
-- Name: available_appointments AppointmentsPK; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.available_appointments
    ADD CONSTRAINT "AppointmentsPK" PRIMARY KEY (appointment_id);


--
-- Name: counties counties_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.counties
    ADD CONSTRAINT counties_pkey PRIMARY KEY (county_id);


--
-- Name: doctors doctors_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.doctors
    ADD CONSTRAINT doctors_pkey PRIMARY KEY (doctor_id);


--
-- Name: hospitals hospitals_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.hospitals
    ADD CONSTRAINT hospitals_pkey PRIMARY KEY (hospital_id);


--
-- Name: insurancecompanies insurancePK; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.insurancecompanies
    ADD CONSTRAINT "insurancePK" PRIMARY KEY (insurancecompany_id);


--
-- Name: medicalhistory medicalhistory_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.medicalhistory
    ADD CONSTRAINT medicalhistory_pkey PRIMARY KEY (medicalhistory_id);


--
-- Name: patients patientsPK; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.patients
    ADD CONSTRAINT "patientsPK" PRIMARY KEY (patient_id);


--
-- Name: policlinics policlinics_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.policlinics
    ADD CONSTRAINT policlinics_pkey PRIMARY KEY (policlinic_id);


--
-- Name: prescriptions prescriptions_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.prescriptions
    ADD CONSTRAINT prescriptions_pkey PRIMARY KEY (prescripton_id);


--
-- Name: provinces provinces_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.provinces
    ADD CONSTRAINT provinces_pkey PRIMARY KEY (province_id);


--
-- Name: unavailable_appointments unavailable_appointments_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.unavailable_appointments
    ADD CONSTRAINT unavailable_appointments_pkey PRIMARY KEY (id);


--
-- Name: users unique_users_social_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT unique_users_social_id UNIQUE (social_id);


--
-- Name: users usersPK; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT "usersPK" PRIMARY KEY (user_id);


--
-- Name: doctors create_available_appointments; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER create_available_appointments AFTER INSERT OR UPDATE ON public.doctors FOR EACH ROW EXECUTE FUNCTION public.create_appointments();


--
-- Name: hospitals create_available_appointments; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER create_available_appointments AFTER INSERT OR UPDATE ON public.hospitals FOR EACH ROW EXECUTE FUNCTION public.create_appointments();


--
-- Name: hospitals hospital_is_deleted; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER hospital_is_deleted AFTER DELETE ON public.hospitals FOR EACH ROW EXECUTE FUNCTION public.delete_hospital();


--
-- Name: unavailable_appointments move_availableappointments; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER move_availableappointments AFTER INSERT OR UPDATE ON public.unavailable_appointments FOR EACH ROW EXECUTE FUNCTION public.move_available_appointments();


--
-- Name: available_appointments move_unavailable_appointments; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER move_unavailable_appointments AFTER UPDATE ON public.available_appointments FOR EACH ROW EXECUTE FUNCTION public.move_unavailable_appointments();


--
-- Name: users newuserRegistered; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER "newuserRegistered" AFTER INSERT ON public.users FOR EACH ROW EXECUTE FUNCTION public.user_registered();


--
-- Name: available_appointments preventDuplicates; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER "preventDuplicates" BEFORE INSERT ON public.available_appointments FOR EACH ROW EXECUTE FUNCTION public.prevent_duplicate_appointments();


--
-- Name: available_appointments preventPassedAppointment; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER "preventPassedAppointment" BEFORE INSERT ON public.available_appointments FOR EACH ROW EXECUTE FUNCTION public.prevent_passed_appointments();


--
-- Name: available_appointments preventPassedAppointmentUpdate; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER "preventPassedAppointmentUpdate" BEFORE UPDATE ON public.available_appointments FOR EACH ROW EXECUTE FUNCTION public.prevent_passed_appointments();


--
-- Name: available_appointments Appointments_DoctorFK; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.available_appointments
    ADD CONSTRAINT "Appointments_DoctorFK" FOREIGN KEY (doctor_id) REFERENCES public.doctors(doctor_id);


--
-- Name: available_appointments Appointments_HospitalFK; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.available_appointments
    ADD CONSTRAINT "Appointments_HospitalFK" FOREIGN KEY (hospital_id) REFERENCES public.hospitals(hospital_id);


--
-- Name: available_appointments Appointments_PatientFK; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.available_appointments
    ADD CONSTRAINT "Appointments_PatientFK" FOREIGN KEY (patient_id) REFERENCES public.patients(patient_id);


--
-- Name: counties CountiesFK; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.counties
    ADD CONSTRAINT "CountiesFK" FOREIGN KEY (province_id) REFERENCES public.provinces(province_id);


--
-- Name: doctors doctorsPoliclinic_FK; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.doctors
    ADD CONSTRAINT "doctorsPoliclinic_FK" FOREIGN KEY (policlinic_id) REFERENCES public.policlinics(policlinic_id);


--
-- Name: doctors doctors_FK; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.doctors
    ADD CONSTRAINT "doctors_FK" FOREIGN KEY (user_id) REFERENCES public.users(user_id);


--
-- Name: doctors hospitalDoctors_FK; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.doctors
    ADD CONSTRAINT "hospitalDoctors_FK" FOREIGN KEY (hospital_id) REFERENCES public.hospitals(hospital_id);


--
-- Name: hospitals hospitalsFK; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.hospitals
    ADD CONSTRAINT "hospitalsFK" FOREIGN KEY (county_id) REFERENCES public.counties(county_id);


--
-- Name: medicalhistory medicalHistoryFK_doctors; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.medicalhistory
    ADD CONSTRAINT "medicalHistoryFK_doctors" FOREIGN KEY (doctor_id) REFERENCES public.doctors(doctor_id);


--
-- Name: medicalhistory medicalHistoryFK_patients; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.medicalhistory
    ADD CONSTRAINT "medicalHistoryFK_patients" FOREIGN KEY (patient_id) REFERENCES public.patients(patient_id);


--
-- Name: medicalhistory medicalHistoryFK_prescriptions; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.medicalhistory
    ADD CONSTRAINT "medicalHistoryFK_prescriptions" FOREIGN KEY (prescripton_id) REFERENCES public.prescriptions(prescripton_id);


--
-- Name: patients patientsUser_FK; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.patients
    ADD CONSTRAINT "patientsUser_FK" FOREIGN KEY (user_id) REFERENCES public.users(user_id);


--
-- Name: policlinics policlinicsFK; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.policlinics
    ADD CONSTRAINT "policlinicsFK" FOREIGN KEY (hospital_id) REFERENCES public.hospitals(hospital_id);


--
-- Name: prescriptions prescriptionFK_doctors; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.prescriptions
    ADD CONSTRAINT "prescriptionFK_doctors" FOREIGN KEY (doctor_id) REFERENCES public.doctors(doctor_id);


--
-- Name: prescriptions prescriptionsFK_patients; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.prescriptions
    ADD CONSTRAINT "prescriptionsFK_patients" FOREIGN KEY (patient_id) REFERENCES public.patients(patient_id);


--
-- Name: unavailable_appointments u_appointmentDoctorFK; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.unavailable_appointments
    ADD CONSTRAINT "u_appointmentDoctorFK" FOREIGN KEY (doctor_id) REFERENCES public.doctors(doctor_id);


--
-- Name: unavailable_appointments u_appointmentPatientFK; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.unavailable_appointments
    ADD CONSTRAINT "u_appointmentPatientFK" FOREIGN KEY (patient_id) REFERENCES public.patients(patient_id);


--
-- Name: users users_countyFK; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT "users_countyFK" FOREIGN KEY (county_id) REFERENCES public.counties(county_id);


--
-- Name: SCHEMA public; Type: ACL; Schema: -; Owner: postgres
--

REVOKE USAGE ON SCHEMA public FROM PUBLIC;


--
-- PostgreSQL database dump complete
--

