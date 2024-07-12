import axios from "axios";

const token =
  "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InRlc3QiLCJuYmYiOjE3MTc1OTE3MzYsImV4cCI6MTcxNzU5NTMzNiwiaWF0IjoxNzE3NTkxNzM2LCJpc3MiOiJ5b3VyZG9tYWluLmNvbSIsImF1ZCI6InlvdXJkb21haW4uY29tIn0.__QmblOONknhwWPYfSsMG8-OFHHZJgQwzmHty-BDWbU";
const url = "https://localhost:7205/";

export const UpsertFormData = async (data, selected) => {
  try {
    const resp = await axios.post(
      `${url}api/PersonalInfor/AddPersonalInfo`,
      {
        firstName: data.firstName,
        lastName: data.lastName,
        email: data.email,
        mobileNo: data.mobile,
        gender: data.gender,
        maritalStatus: data.maritalStatus,
        address: data.address,
        city: data.city,
        state: data.state,
        zip: data.zip,
        race: data.race,
        languages: selected,
      },
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );

    return resp.data;
  } catch (err) {
    return err.response;
  }
};
export const GetLanguages = async () => {
  try {
    const resp = await axios.get(
      `${url}api/PersonalInfor/GetAllLanguages`,

      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );

    return resp.data;
  } catch (err) {
    return err.response;
  }
};
export const GetRaces = async () => {
  try {
    const resp = await axios.get(
      `${url}api/PersonalInfor/GetAllRace`,

      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );

    return resp.data;
  } catch (err) {
    return err.response;
  }
};
export const GetAllProfiles = async () => {
  try {
    const resp = await axios.get(
      `${url}api/PersonalInfor/GetAllPersonalInfo`,

      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );

    return resp.data;
  } catch (err) {
    return err.response;
  }
};
export const DelteProfilesById = async (id) => {
  try {
    const resp = await axios.delete(`${url}api/PersonalInfor/${id}`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });

    return resp.data;
  } catch (err) {
    return err.response;
  }
};

export const UpdateProfileById = async (id, data, languageNames) => {
  try {
    const resp = await axios.put(
      `${url}api/PersonalInfor/${id}`,
      {
        firstName: data.firstName,
        lastName: data.lastName,
        email: data.email,
        mobileNo: data.mobileNo,
        gender: data.gender,
        maritalStatus: data.maritalStatus,
        address: data.address,
        city: data.city,
        state: data.state,
        zip: data.zip,
        race: data.race,
        languages: languageNames,
      },
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );

    return resp.data;
  } catch (err) {
    return err.response;
  }
};
