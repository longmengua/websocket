# 基底影像
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

# 設定工作目錄
WORKDIR /app

# 設置時間zone
ENV TZ=Asia/Taipei

# 安裝必要的套件
RUN apt-get update && \
    apt-get install -y libgdiplus curl libstdc++6 unzip make cmake g++ doxygen python && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# 建構階段
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# 設定工作目錄
WORKDIR /app

# 建置參數
ARG BUILD_CONFIGURATION=Release
ARG ENVIRONMENT=dev

# 複製 csproj 並還原相依性
COPY *.csproj ./
RUN dotnet restore

# 複製整個專案並進行建構
COPY . .
COPY appsettings.${ENVIRONMENT}.json appsettings.json
RUN dotnet publish -c Release -o out

# 最終階段
FROM base AS final
WORKDIR /app

# 複製發佈的檔案到最終影像
COPY --from=build /app/out .

# Set environment variables to use HTTP and specify the port
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80

# 設定進入點
ENTRYPOINT ["dotnet", "NewFunday.dll"]

# 曝露應用程式使用的端口
EXPOSE 80
