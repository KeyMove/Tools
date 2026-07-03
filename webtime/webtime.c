//dll ws2_32.dll
#include <winsock2.h>   // Socket 核心
#include <process.h>    // 按您要求保留
#include <stdio.h>      // printf, sscanf
#include <stdlib.h>     // system
#include <windows.h>    // 必须：SetSystemTime, SYSTEMTIME

// 修复后的解析函数
BOOL ParseHttpDate(const char* dateStr, SYSTEMTIME* st) {
    char month[16];
    int day, year, hour, min, sec;
    // 使用 %*[^,] 跳过 "Fri"，然后匹配逗号和空格
    if (sscanf(dateStr, "%*[^,], %d %s %d %d:%d:%d GMT",
               &day, month, &year, &hour, &min, &sec) != 6) {
        return FALSE;
    }

    const char* months[] = {"Jan","Feb","Mar","Apr","May","Jun",
                            "Jul","Aug","Sep","Oct","Nov","Dec"};
    int monthNum = 0;
    for (int i = 0; i < 12; i++) {
        if (strcmp(month, months[i]) == 0) { monthNum = i + 1; break; }
    }
    if (monthNum == 0) return FALSE;

    st->wYear = (WORD)year;
    st->wMonth = (WORD)monthNum;
    st->wDay = (WORD)day;
    st->wHour = (WORD)hour;
    st->wMinute = (WORD)min;
    st->wSecond = (WORD)sec;
    st->wMilliseconds = 0;
    st->wDayOfWeek = 0;
    return TRUE;
}

// 从响应中提取 Date 头
BOOL ExtractDateHeader(const char* response, char* dateBuf, int bufSize) {
    const char* pattern = "Date: ";
    char* pos = strstr(response, pattern);
    if (!pos) return FALSE;
    pos += strlen(pattern);
    char* end = strstr(pos, "\r\n");
    if (!end) return FALSE;
    int len = (int)(end - pos);
    if (len >= bufSize) len = bufSize - 1;
    strncpy(dateBuf, pos, len);
    dateBuf[len] = '\0';
    return TRUE;
}

// 同步时间主函数
BOOL SyncTimeFromWeb() {
    WSADATA wsaData;
    SOCKET sock = INVALID_SOCKET;
    struct sockaddr_in serverAddr;
    char sendBuf[256] = {0};
    char recvBuf[4096] = {0};
    char dateStr[128] = {0};
    SYSTEMTIME st = {0};
    BOOL success = FALSE;

    if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0) {
        printf("WSAStartup 失败\n");
        return FALSE;
    }

    sock = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
    if (sock == INVALID_SOCKET) {
        printf("socket 创建失败，错误码: %d\n", WSAGetLastError());
        goto cleanup;
    }

    // 域名解析（使用 gethostbyname）
    struct hostent* host = gethostbyname("www.baidu.com");
    if (!host) {
        printf("域名解析失败，错误码: %d\n", WSAGetLastError());
        goto cleanup;
    }
    serverAddr.sin_family = AF_INET;
    serverAddr.sin_port = htons(80);
    serverAddr.sin_addr = *(struct in_addr*)host->h_addr_list[0];

    if (connect(sock, (struct sockaddr*)&serverAddr, sizeof(serverAddr)) == SOCKET_ERROR) {
        printf("连接失败，错误码: %d\n", WSAGetLastError());
        goto cleanup;
    }

    // 构造 HEAD 请求
    sprintf(sendBuf,
        "HEAD / HTTP/1.1\r\n"
        "Host: www.baidu.com\r\n"
        "Connection: close\r\n"
        "\r\n");

    if (send(sock, sendBuf, (int)strlen(sendBuf), 0) == SOCKET_ERROR) {
        printf("发送请求失败，错误码: %d\n", WSAGetLastError());
        goto cleanup;
    }

    // 接收响应头
    int totalRecv = 0, bytes;
    while (totalRecv < (int)sizeof(recvBuf) - 1) {
        bytes = recv(sock, recvBuf + totalRecv, sizeof(recvBuf) - totalRecv - 1, 0);
        if (bytes <= 0) break;
        totalRecv += bytes;
        recvBuf[totalRecv] = '\0';
        if (strstr(recvBuf, "\r\n\r\n") != NULL) break;
    }
    if (totalRecv == 0) {
        printf("未收到数据\n");
        goto cleanup;
    }

    if (!ExtractDateHeader(recvBuf, dateStr, sizeof(dateStr))) {
        printf("未找到 Date 头\n");
        goto cleanup;
    }
    printf("服务器时间: %s\n", dateStr);

    if (!ParseHttpDate(dateStr, &st)) {
        printf("日期解析失败\n");
        goto cleanup;
    }
    printf("解析结果: %04d-%02d-%02d %02d:%02d:%02d (UTC)\n",
           st.wYear, st.wMonth, st.wDay,
           st.wHour, st.wMinute, st.wSecond);

    // 设置系统时间（需要管理员权限）
    if (!SetSystemTime(&st)) {
        printf("SetSystemTime 失败，错误码: %lu\n", GetLastError());
        printf("请以管理员身份运行此程序！\n");
        goto cleanup;
    }

    printf("系统时间同步成功！\n");
    success = TRUE;

cleanup:
    if (sock != INVALID_SOCKET) closesocket(sock);
    WSACleanup();
    return success;
}

int main() {
    printf("=== 网络时间同步（Socket 版）===\n");
    printf("正在从 www.baidu.com 获取时间...\n\n");

    if (SyncTimeFromWeb())
        printf("\n操作完成！\n");
    else
        printf("\n操作失败！\n");

    system("pause");
    return 0;
}