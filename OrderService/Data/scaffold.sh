#!/bin/bash
dotnet ef dbcontext scaffold \
    "Server=localhost;Port=5432;Database=appdb;User Id=app;Password=secret" \
    Npgsql.EntityFrameworkCore.PostgreSQL \
    --output-dir Models \
    --context-dir Data \
    --context OrderContext \
    --no-onconfiguring \
    --data-annotations \
    --force