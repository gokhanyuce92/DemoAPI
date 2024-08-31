# Demo API
**Demo API**, TCMB kur servisinden günlük döviz kurlarını çeken bir REST API projesidir. C# ve .NET 8 kullanılarak geliştirilmiştir ve token tabanlı kimlik doğrulama (JWT) kullanır.
## Özellikler
* TCMB kur servisinden günlük döviz kurlarını çeker.
* JSON formatında veri döndürür.
* Token tabanlı kimlik doğrulama (JWT) ile güvenli erişim sağlar.
## Teknolojiler
* C#
* .NET 8
## Kurulum
1. Projeyi klonlayın:
2. Gerekli bağımlılıkları yükleyin:
3. `appsettings.json` dosyasında gerekli yapılandırmaları yapın (örneğin, veritabanı bağlantı dizesi, JWT ayarları vb.).
4. Uygulamayı çalıştırın:
## Kullanım
API uç noktalarına erişmek için bir JWT token'ı almanız gerekir. Token'ı almak için `api/auth/login` uç noktasına geçerli kullanıcı adı ve şifresiyle bir POST isteği gönderin.
Döviz kurlarını çekmek için `api/currency` uç noktasına bir GET isteği gönderin. İstek başlığında `Authorization` alanına `Bearer <token>` şeklinde JWT token'ınızı eklemeniz gerekir.