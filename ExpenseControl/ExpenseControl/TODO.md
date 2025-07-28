# ✅ TODO - Projeto Controle de Despesas (.NET MAUI)

## Funcionalidades Pendentes

- [ ] Criar funcionalidade para apagar despesas através de filtros de datas.
- [ ] Criar sistema de login de usuário para sincronização futura com banco de dados online.
- [ ] Implementar sincronização dos dados entre dispositivos (API REST).
- [ ] Permitir configuração de banco local ou banco online híbrido.

---

## Melhorias Planejadas

### 🔄 Filtro na exportação de dados

- [x] Exportar por mês específico
- [ ] Exportar por intervalo de datas personalizado
- [ ] Permitir selecionar categorias específicas para exportação

### 📊 Otimizar visualização de categorias na consolidação

- [ ] Botão para consolidar despesas após selecionar os filtros
- [x] Zerar despesas ao selecionar período sem despesas
- [ ] Agrupar categorias com baixo valor individual em "Outras despesas"
- [ ] Exibir individualmente apenas as principais categorias

### 🔍 Filtros avançados na listagem de despesas *(parcialmente implementado)*

- [x] Buscar por descrição
- [x] Filtrar por valor mínimo e máximo
- [x] Filtro por categoria (ajustado com opção "Todas")
- [ ] Filtro por intervalo de datas
- [ ] Buscar por valor exato, maior que, menor que, entre
- [ ] Melhorar filtros combinados e usabilidade (ex: múltiplos critérios)

### 💳 Separar despesas por forma de pagamento

- [ ] Incluir campo para tipo de pagamento: cartão de crédito, dinheiro ou saldo bancário
- [ ] Permitir visualização e filtragem por tipo de pagamento

---

## Ajustes Estéticos

- [ ] Alterar ícone de identidade do aplicativo
- [x] Corrigir corte de nome das páginas no menu de navegação (Flyout)
- [x] Corrigir tema escuro automático no Android apk.
- [x] Melhorar adaptação do layout para telas menores (ex. dispositivos Android)

---

## Documentação

- [ ] Criar documentação completa do projeto
- [ ] Adicionar instruções de build e publicação para Android e Windows

---
## Build Android (APK)

```bash
dotnet publish -f:net8.0-android -c Release -p:AndroidPackageFormat=apk