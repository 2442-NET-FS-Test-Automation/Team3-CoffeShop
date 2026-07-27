import type { ReactNode } from "react";
import { Navigate } from "react-router-dom";
import { useAuth } from "./useAuth";

type RequireAuthProps = {
    children: ReactNode;
    allowedRoles?: string[];
};

function RequireAuth({ children, allowedRoles }: RequireAuthProps) {
    const { status, user } = useAuth();

    if (status !== "authenticated") {
        return <Navigate to="/login" replace />;
    }

    if (allowedRoles && !allowedRoles.includes(user?.role ?? "")) {
        return <Navigate to="/login" replace />;
    }

    return <>{children}</>;
}

export default RequireAuth;
