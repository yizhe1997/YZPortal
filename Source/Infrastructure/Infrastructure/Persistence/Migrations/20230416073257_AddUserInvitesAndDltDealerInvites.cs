using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserInvitesAndDltDealerInvites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DealerInvites");

            migrationBuilder.CreateTable(
                name: "UserInvites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CallbackUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClaimedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ValidUntilDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Token = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "unknown"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "unknown"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FailedMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FailedSentDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Attempts = table.Column<int>(type: "int", nullable: false),
                    LastAttemptedSentDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInvites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserInvites_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserInviteDealerSelection",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    DealerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserRole = table.Column<int>(type: "int", nullable: false),
                    UserContentAccessLevels = table.Column<int>(type: "int", nullable: false),
                    MembershipId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserInviteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "unknown"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "unknown")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInviteDealerSelection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserInviteDealerSelection_Dealers_DealerId",
                        column: x => x.DealerId,
                        principalTable: "Dealers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserInviteDealerSelection_UserInvites_UserInviteId",
                        column: x => x.UserInviteId,
                        principalTable: "UserInvites",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserInviteDealerSelection_DealerId",
                table: "UserInviteDealerSelection",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInviteDealerSelection_UserInviteId",
                table: "UserInviteDealerSelection",
                column: "UserInviteId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInvites_UserId",
                table: "UserInvites",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserInviteDealerSelection");

            migrationBuilder.DropTable(
                name: "UserInvites");

            migrationBuilder.CreateTable(
                name: "DealerInvites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    DealerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Attempts = table.Column<int>(type: "int", nullable: false),
                    CallbackUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClaimedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "unknown"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FailedMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FailedSentDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastAttemptedSentDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MembershipId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Token = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "unknown"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UserContentAccessLevels = table.Column<int>(type: "int", nullable: false),
                    UserRole = table.Column<int>(type: "int", nullable: false),
                    ValidUntilDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerInvites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealerInvites_Dealers_DealerId",
                        column: x => x.DealerId,
                        principalTable: "Dealers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DealerInvites_DealerId",
                table: "DealerInvites",
                column: "DealerId");
        }
    }
}
