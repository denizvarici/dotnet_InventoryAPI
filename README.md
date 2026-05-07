## Dotnet Inventory API

Bu proje bir **Stok Yönetim Sistemi**'dir.

Proje, çeşitli mimari desenlerin (CQRS, Event-Driven Design) bir stok yönetim sistemi üzerinde uygulanmasını tecrübe etmek amacıyla geliştirilmiştir.

## Mimari

* **Clean Architecture:** Proje katmanlara ayrılarak SoC(Seperation of Concerns) prensibine tam uyumlu hale getirilmiştir.
* **Custom CQRS (MediatR kullanılmadı):** Dış bir kütüphane kullanılmadan, özel `ICommandHandler` ve `IQueryHandler` arayüzleri yazılarak komut ve sorgu işlemleri birbirinden ayrılmıştır.
* **İki farklı ORM:**
    * **Command İşlemleri:** Veri bütünlüğünü sağlamak için Entity Framework Core ve PostgreSQL Transaction yapıları kullanılmıştır.
    * **Query Şşlemleri:** Yüksek performans için `ISqlConnectionFactory` üzerinden ham SQL ve **Dapper** kullanılmıştır.
* **Domain-Driven Design (DDD) & Domain Events:** Rich domain models yapısı geliştirildi ve stok kritik eşiğe düştüğünde (`LowStockDetectedEvent`) tetiklenen eventler, özel yazılmış bir EF Core Interceptor ile yakalanmıştır.
* **GraphQL & Gerçek Zamanlı Bildirimler (WebSockets):** * REST API'nin yanı sıra okuma operasyonları için **HotChocolate** GraphQL entegrasyonu yapılmıştır.
    * Stok azaldığında çalışan Domain Event'ler, GraphQL **Subscriptions** ve WebSockets üzerinden anında istemcilere real-time bildirim olarak iletilmektedir.
* **Kapsamlı Test Altyapısı:** * İş kuralları **xUnit**, **Moq** ve **FluentAssertions** ile Unit Test'lerle güvenceye alınmıştır.
    * **Testcontainers** kullanılarak, testler her çalıştığında arka planda gerçek bir PostgreSQL Docker container'ı ayağa kaldıran end-to-end integration tests yazılmıştır.

## 🛠 Kullanılan Teknolojiler

* **.NET 10.0**
* **PostgreSQL**
* **Entity Framework Core** & **Dapper**
* **HotChocolate**
* **Scalar** 
* **Testcontainers**, **xUnit**, **Moq**, **FluentAssertions**
