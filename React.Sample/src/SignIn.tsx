import React from "react";
import { useForm } from "react-hook-form";
import { UserCredentials } from "seetrue.client";
import { useSeeTrue } from "seetrue.client.react";
import { useLocation } from "wouter";

export const Login: React.FC = () => {
  const [loading, setLoading] = React.useState(false);
  const { client } = useSeeTrue();
  const [_location, setLocation] = useLocation();

  const { register, handleSubmit } = useForm<UserCredentials>({
    defaultValues: {
      email: "",
      password: "",
    },
  });

  const onSubmit = async (data: UserCredentials) => {
    try {
      setLoading(true);
      client.login(data);
    } catch (error) {
      console.log(error);
    } finally {
      setLoading(false);
    }
  };

  const onRegister = () => {
    setLocation("/signup");
  };

  return (
    <div
      style={{
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        height: "100vh",
      }}
    >
      <form
        onSubmit={handleSubmit(onSubmit)}
        style={{ display: "flex", flexDirection: "column", width: "400px" }}
      >
        <label htmlFor="email">Email</label>
        <input type="email" {...register("email")} />
        <label htmlFor="password">Password</label>
        <input type="password" {...register("password")} />
        <button type="submit" disabled={loading}>
          {loading ? "Loading" : "Login"}
        </button>
        <button onClick={onRegister} type="button">
          Sign up
        </button>
      </form>
    </div>
  );
};
